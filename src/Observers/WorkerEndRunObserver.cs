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
    public class WorkerEndRunObserver : WorkerObserver
    {
        //private static object nb_LOCK = new object();
        private ILogger<WorkerEndRunObserver> logger;
        public WorkerEndRunObserver() : base(Enums.WorkerEvents.EndRun, 10)
        {

        }

        public override Task Todo(WorkerContext _context, BrunContext brunContext)
        {
            logger = _context.ServiceProvider.GetRequiredService<ILogger<WorkerEndRunObserver>>();
            long end = Interlocked.Increment(ref _context.endNb);
            logger.LogDebug("backrun:{0} is end,startNb:{1} endNb:{2} {3},thread id:{4}", brunContext.BrunType.Name, brunContext.StartNb, end, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Thread.CurrentThread.ManagedThreadId);
            return Task.CompletedTask;
        }
    }
}
