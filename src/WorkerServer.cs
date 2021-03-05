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

namespace Brun
{
    /// <summary>
    /// 进程单例，暂时只考虑单机运行
    /// </summary>
    public sealed class WorkerServer : IWorkerServer
    {
        private WorkerServerConfig serverConfig = new WorkerServerConfig();
        private static WorkerServer _workerServer;
        private static object serverCreate_LOCK = new object();
        private IList<IWorker> worders = new List<IWorker>();
        private IServiceProvider _serviceProvider;
        private CancellationToken _stoppingToken;
        ILoggerFactory loggerFactory;
        ILogger<WorkerServer> logger;
        private TaskFactory taskFactory;
        private WorkerServer()
        {

        }
        /// <summary>
        /// server配置，用于设置系统默认配置
        /// </summary>
        public WorkerServerConfig ServerConfig => serverConfig;
        /// <summary>
        /// 所有运行中的worker
        /// </summary>
        public IList<IWorker> Worders => worders;
        /// <summary>
        /// 服务容器
        /// </summary>
        public IServiceProvider ServiceProvider => _serviceProvider;
        /// <summary>
        /// 服务结束Token
        /// </summary>
        public CancellationToken StoppingToken => _stoppingToken;
        /// <summary>
        /// 公共的Task管理
        /// </summary>
        public TaskFactory TaskFactory => taskFactory;
        public IWorker GetWorker(string key)
        {
            IWorker worker = worders.FirstOrDefault(m => m.Key == key);
            if (worker == null)
            {
                logger.LogError("找不到活动的Worker，key:{0}", key);
            }
            return worker;
        }
        public IEnumerable<IWorker> GetWokerByName(string name)
        {
            return worders.Where(m => m.Name == name);
        }
        public IEnumerable<IWorker> GetWokerByTag(string tag)
        {
            return worders.Where(m => m.Tag == tag);
        }
        public IOnceWorker GetOnceWorker(string key)
        {
            IWorker worker = worders.Where(m => m.Context.Option.WorkerType == typeof(OnceWorker)).FirstOrDefault(m => m.Key == key);
            if (worker == null)
            {
                logger.LogError("找不到活动的OnceWorker，key:{0}", key);
            }
            return (IOnceWorker)worker;
        }
        public IQueueWorker GetQueueWorker(string key)
        {
            IWorker worker = worders.Where(m => m.Context.Option.WorkerType == typeof(QueueWorker)).FirstOrDefault(m => m.Key == key);
            if (worker == null)
            {
                logger.LogError("找不到活动的QueueWorker，key:{0}", key);
            }
            return (IQueueWorker)worker;
        }
        /// <summary>
        /// 启动Brun
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="stoppingToken"></param>
        public void Start(IServiceProvider serviceProvider, CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            taskFactory = new TaskFactory(_stoppingToken);
            ServiceConfigure(serviceProvider);

            foreach (var item in worders.Where(m => m is IQueueWorker))
            {
                Thread queue = new Thread(new ThreadStart(((IQueueWorker)item).Start().Wait));
                //Thread queue = new Thread(new ParameterizedThreadStart(((IQueueWorker)item).Start));
                //queue.IsBackground = true;
                //queue.Start(_stoppingToken);
                //((IQueueWorker)item).Start();//.Run();
            }
            foreach (var item in worders.Where(m => m is ITimeWorker))
            {
                Thread time = new Thread(new ThreadStart(((ITimeWorker)item).Start().Wait));
                //Thread time = new Thread(new ParameterizedThreadStart(((ITimeWorker)item).Start));
                //time.IsBackground = true;
                //time.Start(_stoppingToken);
                //((ITimeWorker)item).Start();
            }
            logger.LogInformation("WorkerServer is Started");
            stoppingToken.Register(() => Stop());
        }
        private void ServiceConfigure(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(IServiceProvider), "serviceProvider can not be null");
            }
            _serviceProvider = serviceProvider;
            WorkerConfigure();
            loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            logger = loggerFactory.CreateLogger<WorkerServer>();
        }
        private void WorkerConfigure()
        {

        }

        public void Stop()
        {
            logger.LogDebug("WorkerServer is Stopping! please wait workers dispose...");
            //此处用于处理所有worker注销
            foreach (var item in worders)
            {
                item.Dispose();
            }

            logger.LogDebug("WorkerServer is Stoped");
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
