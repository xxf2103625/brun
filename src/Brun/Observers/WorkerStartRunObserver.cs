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
        private static ILogger<WorkerStartRunObserver> logger;
        private IBackRunObserverService backRunDetailService;
        public WorkerStartRunObserver() : base(Enums.WorkerEvents.StartRun, 10)
        {
        }
        public override Task Todo(BrunContext brunContext)
        {
            brunContext.StartDateTime = DateTime.Now;
            if (logger == null)
            {
                logger = brunContext.WorkerContext.LoggerFactory.CreateLogger<WorkerStartRunObserver>();
            }
            long start = Interlocked.Increment(ref brunContext.WorkerContext.startNb);
            if (backRunDetailService == null)
                backRunDetailService = brunContext.WorkerContext.ServiceProvider.GetRequiredService<IBackRunObserverService>();
            backRunDetailService.Start(brunContext);
            logger.LogTrace("backrun:{0} is start,startNb:{1},time:{2},thread id:{3}", brunContext.BackRun.GetType().Name, start, brunContext.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss FFFF"), Thread.CurrentThread.ManagedThreadId);
            return Task.CompletedTask;
        }
    }
}
