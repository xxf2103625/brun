using Brun.BaskRuns;
using Brun.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    /// <summary>
    /// BackRun基础基类，每种Worker独立分开
    /// </summary>
    public abstract class BackRun : IBackRun
    {
        private WorkerContext _workerContext;
        protected BackRunOption option;
        internal long startNb = 0;
        internal long errorNb = 0;
        internal long endNb = 0;
        internal string lastErrorId = null;
        public BackRun(BackRunOption option)
        {
            this.option = option;
        }
        /// <summary>
        /// 共享的自定义数据，修改时请自己加锁
        /// </summary>
        public ConcurrentDictionary<string, string> Data => _workerContext.Items;
        public string Id => option.Id;
        public string Name => option.Name;
        public long StartTimes => startNb;
        public long ErrorTimes => errorNb;
        public long EndTimes => endNb;
        public string LastErrorId => lastErrorId;
        public WorkerContext WorkerContext => _workerContext;
        public IServiceProvider ServiceProvider => _workerContext.ServiceProvider;
        public TService GetRequiredService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }
        public TService GetService<TService>()
        {
            return ServiceProvider.GetService<TService>();
        }
        public object GetService(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }
        public IServiceScope CreateScope()
        {
            return ServiceProvider.CreateScope();
        }
        public AsyncServiceScope CreateAsyncScope(Type serviceType)
        {
            return ServiceProvider.CreateAsyncScope();
        }
        internal void SetWorkerContext(WorkerContext workerContext)
        {
            this._workerContext = workerContext;
        }
    }
}
