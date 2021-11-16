﻿using Brun.Observers;
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
        private DateTime? startTime = null;
        private IServiceProvider _serviceProvider;
        ILoggerFactory loggerFactory;
        ILogger<WorkerServer> logger;
        private WorkerServer()
        {

        }
        public Action<WorkerServer> Configure { get; set; }
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
        /// 
        /// </summary>
        public ILoggerFactory LoggerFactory => loggerFactory;
        public void SetLogFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }
        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        public IWorker GetWorker(string key)
        {
            IWorker worker = worders.FirstOrDefault(m => m.Key == key);
            if (worker == null)
            {
                logger.LogError("can not find active Worker，key:'{0}'", key);
            }
            return worker;
        }
        public IList<IWorker> GetAllWorker()
        {

            return worders.Where(m => m.Key != SystemBackRun.Worker_KEY).ToList();
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
            IWorker worker = worders.Where(m => m.GetType() == typeof(OnceWorker)).FirstOrDefault(m => m.Key == key);
            if (worker == null)
            {
                logger.LogError("can not find active OnceWorker，key:'{0}'", key);
            }
            return (IOnceWorker)worker;
        }
        public IQueueWorker GetQueueWorker(string key)
        {
            IWorker worker = worders.FirstOrDefault(m => m.Key == key);
            if (worker == null)
            {
                logger.LogError("can not find active QueueWorker，key:'{0}'", key);
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
            ServiceConfigure(serviceProvider);
            logger.LogInformation("WorkerServer is Started");
            startTime = DateTime.Now;
            foreach (var item in worders)
            {
                item.Start();
            }
            stoppingToken.Register(() => Stop());
        }
        private void ServiceConfigure(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(IServiceProvider), "serviceProvider can not be null");
            }
            //_serviceProvider = serviceProvider;
            WorkerConfigure();
            //loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            logger = loggerFactory.CreateLogger<WorkerServer>();
        }
        private void WorkerConfigure()
        {
            //TODO WorkerConfigure
        }

        public void Stop()
        {
            logger?.LogDebug("WorkerServer is Stopping! please wait workers dispose...");
            //此处用于处理所有worker注销
            foreach (var item in worders)
            {
                item.Dispose();
            }
            this.worders.Clear();
            logger?.LogDebug("WorkerServer is Stoped");
        }
        public DateTime? StartTime => startTime;
        public Plan.IPlanTimeParser PlanTimeParser { get; set; } = new Plan.CroParser();
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
        /// <summary>
        /// 清理单例，单元测试专用
        /// </summary>
        public static void ClearInstance()
        {
            lock (serverCreate_LOCK)
            {
                _workerServer = null;
            }
        }
    }
}
