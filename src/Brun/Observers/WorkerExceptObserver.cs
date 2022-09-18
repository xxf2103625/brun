using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Observers
{
    public class WorkerExceptObserver : WorkerObserver
    {
        public WorkerExceptObserver() : base(WorkerEvents.Except, 10)
        {

        }
        public override Task Todo(BrunContext brunContext)
        {
            Interlocked.Increment(ref brunContext.BackRun.WorkerContext.exceptNb);
            return Task.CompletedTask;
        }
    }
}
