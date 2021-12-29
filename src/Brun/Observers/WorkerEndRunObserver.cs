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
        //private static object nb_LOCK = new object();
        private static ILogger<WorkerEndRunObserver> logger;
        private IBackRunObserverService backRunDetailService;
        public WorkerEndRunObserver() : base(Enums.WorkerEvents.EndRun, 10)
        {

        }

        public override Task Todo(BrunContext brunContext)
        {
            brunContext.EndDateTime = DateTime.Now;
            if (logger == null)
            {
                logger = brunContext.WorkerContext.LoggerFactory.CreateLogger<WorkerEndRunObserver>();
            }
            long end = Interlocked.Increment(ref brunContext.WorkerContext.endNb);
            if (backRunDetailService == null)
                backRunDetailService = brunContext.WorkerContext.ServiceProvider.GetRequiredService<IBackRunObserverService>();
            backRunDetailService.End(brunContext);
            logger.LogTrace("backrun:{0} is end,startNb:{1},endNb:{2},time:{3},thread id:{4}", brunContext.BackRun.GetType().Name, brunContext.StartNb, end, brunContext.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss"), Thread.CurrentThread.ManagedThreadId);
            return Task.CompletedTask;
        }
    }
}
