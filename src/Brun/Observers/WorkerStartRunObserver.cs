using Brun.Commons;
using Brun.Contexts;
using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Observers
{
    /// <summary>
    /// BackRun开始执行前的拦截器
    /// </summary>
    public class WorkerStartRunObserver : WorkerObserver
    {
        public WorkerStartRunObserver() : base(Enums.WorkerEvents.StartRun, 10)
        {
        }
        public override Task Todo(BrunContext brunContext)
        {
            brunContext.StartDateTime = DateTime.Now;
            Interlocked.Increment(ref brunContext.WorkerContext.startNb);
            return Task.CompletedTask;
        }
    }
}
