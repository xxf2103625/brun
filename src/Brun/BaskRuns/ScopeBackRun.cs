using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    /// <summary>
    /// 每次运行在Ioc的Scope中，可以理解成类似mvc每个请求的生命周期
    /// </summary>
    public abstract class ScopeBackRun : IBackRun
    {
        //TODO 添加options
        public string Id => this.GetType().FullName;
        public string Name => this.GetType().Name;
        /// <summary>
        /// 原始ServiceProvider
        /// </summary>
        private IServiceProvider BaseServiceProvider => WorkerServer.Instance.ServiceProvider;
        /// <summary>
        /// Scope内的ServiceProvider
        /// </summary>
        protected IServiceProvider ServiceProvider;
        /// <summary>
        /// 每次运行共享的自定义数据，修改请自己加锁
        /// </summary>
        public ConcurrentDictionary<string, string> Data { get; set; }

        public WorkerContext WorkerContext => throw new NotImplementedException();

        /// <summary>
        /// 每次会创建一个Scope，ServiceProvider变为Scope内的ServiceProvider，可以理解成类似mvc每个请求的生命周期
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public abstract Task RunInScope(CancellationToken stoppingToken);
        /// <summary>
        /// 运行入口
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task Run(CancellationToken stoppingToken)
        {
            using (IServiceScope scope = BaseServiceProvider.CreateScope())
            {
                ServiceProvider = scope.ServiceProvider;
                await RunInScope(stoppingToken);
            }
        }
        /// <summary>
        /// 获取Ioc注入的Service,找不到会异常，可以获取所有Ioc的Service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetRequiredService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }
        /// <summary>
        /// 获取Ioc注入的Service,可能返回null，可以获取所有Ioc的Service
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetService<TService>()
        {
            return ServiceProvider.GetService<TService>();
        }

        public void SetWorkerContext(WorkerContext workerContext)
        {
            throw new NotImplementedException();
        }
    }
}
