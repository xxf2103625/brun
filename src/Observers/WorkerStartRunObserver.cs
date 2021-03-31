using Brun.Commons;
using Brun.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Observers
{
    public class WorkerStartRunObserver : WorkerObserver
    {
        //private static object nb_LOCK = new object();
        private ILogger<WorkerStartRunObserver> logger;
        public WorkerStartRunObserver() : base(Enums.WorkerEvents.StartRun, 10)
        {

        }

        public override Task Todo(WorkerContext _context, BrunContext brunContext)
        {
            logger = _context.ServiceProvider.GetRequiredService<ILogger<WorkerStartRunObserver>>();
            long start= Interlocked.Increment(ref _context.startNb);
            brunContext.StartNb = start;
            logger.LogDebug("backrun:{0} is start,startNb:{1} {2},thread id:{3}", brunContext.BrunType.Name, start, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss FFFF"),Thread.CurrentThread.ManagedThreadId);
            return Task.CompletedTask;
        }
    }
}
