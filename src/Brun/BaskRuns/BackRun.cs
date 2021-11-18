using Brun.BaskRuns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 包含容器和自定义数据的单次后台任务
    /// </summary>
    public abstract class BackRun : BackRunServicePrivoder, IBackRun
    {
        private WorkerContext _workerContext;
        /// <summary>
        /// 共享的自定义数据，修改时请自己加锁
        /// </summary>
        public ConcurrentDictionary<string, string> Data => _workerContext.Items;

        public WorkerContext WorkerContext => _workerContext;

        public virtual string Id { get; set; }
        /// <summary>
        /// 定义长时间任务时，自己用stoppingToken控制任务尽快结束
        /// </summary>
        /// <param name="stoppingToken">结束信号</param>
        /// <returns></returns>
        public abstract Task Run(CancellationToken stoppingToken);
        public void SetWorkerContext(WorkerContext workerContext)
        {
            this._workerContext = workerContext;
        }
    }
}
