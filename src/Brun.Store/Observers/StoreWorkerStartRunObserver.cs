using Brun.Contexts;
using Brun.Enums;
using Brun.Observers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Store.Observers
{
    /// <summary>
    /// BackRun执行前的持久化处理
    /// </summary>
    public class StoreWorkerStartRunObserver : WorkerObserver
    {
        public StoreWorkerStartRunObserver(WorkerEvents workerEvent) : base(workerEvent, 20)
        {
        }

        public override Task Todo(BrunContext brunContext)
        {
            //TODO 持久化
            IServiceScope scope = brunContext.CreateScope();
            return Task.CompletedTask;
        }
    }
}

