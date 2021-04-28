using Brun.Contexts;
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
    public class SynchroBeforRunObserver : WorkerObserver
    {
        public SynchroBeforRunObserver() : base(Enums.WorkerEvents.StartRun, 5)
        {

        }
        public override Task Todo(WorkerContext _context, BrunContext brunContext)
        {
            var logger = _context.ServiceProvider.GetRequiredService<ILogger<WorkerStartRunObserver>>();
            logger.LogTrace("SynchroBeforRunObserver,start:{0},end:{1}", brunContext.StartNb, _context.endNb);
            if (brunContext.StartNb > _context.endNb)
            {
                Thread.Sleep(5);
            }
            return Task.CompletedTask;
        }
    }
}
