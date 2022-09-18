using Brun.Observers;
using Brun;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Brun.Workers;
using Brun.Services;
using Brun.Exceptions;
using System.IO;

namespace Brun
{
    /// <summary>
    /// 进程单例，暂时只考虑单机运行
    /// </summary>
    public sealed class WorkerServer
    {
        private WorkerServerConfig serverConfig = new WorkerServerConfig();
        private static WorkerServer _workerServer;
        //单例锁
        private static object serverCreate_LOCK = new object();
        private IDictionary<string, IWorker> worders = new Dictionary<string, IWorker>();
        private DateTime? startTime = null;
        private IServiceProvider _serviceProvider;
        ILoggerFactory loggerFactory;
        ILogger<WorkerServer> logger;
        private WorkerServer()
        {

        }
        /// <summary>
        /// 配置
        /// </summary>
        public WorkerServerOption Option { get; set; }
        /// <summary>
        /// server配置，用于设置系统默认配置
        /// </summary>
        public WorkerServerConfig ServerConfig => serverConfig;
        /// <summary>
        /// 所有运行中的worker，不要直接操作这个
        /// </summary>
        public IDictionary<string, IWorker> Worders => worders;
        /// <summary>
        /// IOC容器
        /// </summary>
        public IServiceProvider ServiceProvider => _serviceProvider;
        /// <summary>
        /// 日志工厂
        /// </summary>
        public ILoggerFactory LoggerFactory => loggerFactory;
        /// <summary>
        /// WorkerServer在本次进程中的 启动时间
        /// </summary>
        public DateTime? StartTime => startTime;
        /// <summary>
        /// 计划时间计算器，暂时只实现了Cro表达式.
        /// TODO Cron表达式解析及计算
        /// </summary>
        public Plan.IPlanTimeParser PlanTimeParser { get; set; } = new Plan.CroParser();

        /// <summary>
        /// 配合单元测试,避免任务还没开始 host进程直接结束
        /// </summary>
        public static bool BrunIsStart { get; set; } 
        /// <summary>
        /// 必须配置初始化
        /// </summary>
        /// <param name="serviceProvider">Ioc容器</param>
        public void Init(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
            this.loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
            this.logger = loggerFactory.CreateLogger<WorkerServer>();
        }
        /// <summary>
        /// 可选，自定义日志工厂
        /// </summary>
        /// <param name="loggerFactory"></param>
        public void SetLogFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            this.logger = loggerFactory.CreateLogger<WorkerServer>();
        }
        /// <summary>
        /// 启动Brun Server
        /// </summary>
        /// <param name="stoppingToken"></param>
        public void Start(CancellationToken stoppingToken)
        {
            //ServiceConfigure(serviceProvider);
            if (this._serviceProvider == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "the Ioc ServiceProvider is null,plz use Init() to set it");
            startTime = DateTime.Now;
            logger.LogInformation("WorkerServer is Started");
            if (Option.CreateLinuxPidFile)
                CreatePidFile();
            stoppingToken.Register(() => Stop());
        }

        /// <summary>
        /// 结束Brun Server
        /// </summary>
        public void Stop()
        {
            logger?.LogDebug("WorkerServer is Stopping! please wait workers dispose...");
            //此处用于处理所有内存对象worker注销
            foreach (var item in worders)
            {
                item.Value.Dispose();
            }
            this.worders.Clear();
            if (Option.CreateLinuxPidFile)
                DeletePidFile();
            logger?.LogDebug("WorkerServer is Stoped");
        }
        /// <summary>
        /// 创建Linux下Pid文件
        /// </summary>
        private void CreatePidFile()
        {
            if (Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".pid");
                logger.LogInformation($"auto create Process pid file to:'{filePath}'.");
                byte[] buffer = System.Text.UTF8Encoding.UTF8.GetBytes(pid.ToString());
                FileStream pidfile = File.Create(filePath, buffer.Length, FileOptions.Asynchronous);
                pidfile.Write(buffer, 0, buffer.Length);
                pidfile.Flush(true);
            }
        }
        /// <summary>
        /// 删除Linux下Pid文件
        /// </summary>
        private void DeletePidFile()
        {
            if (Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                int pid = System.Diagnostics.Process.GetCurrentProcess().Id;
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.FriendlyName + ".pid");
                logger.LogInformation($"delete Process pid file,path:'{filePath}'.");
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    logger.LogError("delete Process pid error,message:'{0}'", ex.Message);
                }
            }
        }
        /// <summary>
        /// 进程单例
        /// </summary>
        public static WorkerServer Instance
        {
            get
            {
                if (_workerServer == null)
                {
                    lock (serverCreate_LOCK)
                    {
                        if (_workerServer == null)
                            _workerServer = new WorkerServer();
                    }
                }
                return _workerServer;
            }
        }
    }
}
