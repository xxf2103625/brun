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
    public class WorkerEndRunObserver : WorkerObserver
    {
        public WorkerEndRunObserver() : base(Enums.WorkerEvents.EndRun, 10)
        {

        }

        public override Task Todo(BrunContext brunContext)
        {
            brunContext.EndDateTime = DateTime.Now;
            Interlocked.Increment(ref brunContext.WorkerContext.endNb);
            return Task.CompletedTask;
        }
    }
}
