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
using System.Threading.Tasks;

namespace Brun.Observers
{
    public class WorkerExceptObserver : WorkerObserver
    {
        private static ILogger<WorkerExceptObserver> logger;
        public WorkerExceptObserver() : base(WorkerEvents.Except, 10)
        {

        }
        public override Task Todo(BrunContext brunContext)
        {
            if (logger == null)
            {
                logger = brunContext.WorkerContext.LoggerFactory.CreateLogger<WorkerExceptObserver>();
            }
            System.Threading.Interlocked.Increment(ref brunContext.BackRun.WorkerContext.exceptNb);
            //brunContext.ExceptNb = _context.exceptNb;
            IBackRunObserverService backRunlService = brunContext.WorkerContext.ServiceProvider.GetRequiredService<IBackRunObserverService>();
            backRunlService.Except(brunContext);
            logger.LogError(" backRun:{0} is except!count:{1},msg:{2},StackTrace:{3},time:{4}", brunContext.BackRun.GetType().Name, brunContext.WorkerContext.exceptNb, brunContext.Exception.Message, brunContext.Exception.StackTrace, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return Task.CompletedTask;
        }
    }
}
