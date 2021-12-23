using Brun.BaskRuns;
using Brun.Options;
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
    public abstract class BackRun : BackRunServicePrivoder, IBackRun
    {
        private WorkerContext _workerContext;
        protected BackRunOption option;
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
        public WorkerContext WorkerContext => _workerContext;
        internal void SetWorkerContext(WorkerContext workerContext)
        {
            this._workerContext = workerContext;
        }
    }
}
