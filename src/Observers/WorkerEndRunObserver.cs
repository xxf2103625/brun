using Brun.Commons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Observers
{
    public class WorkerEndRunObserver : WorkerObserver
    {
        private static object nb_LOCK = new object();
        private ILogger<WorkerEndRunObserver> logger;
        public WorkerEndRunObserver() : base(Enums.WorkerEvents.EndRun, 10)
        {
            
        }
        public override Task Todo(WorkerContext _context)
        {
            logger = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<WorkerEndRunObserver>>();
            lock (nb_LOCK)
            {
                _context.endNb++;
            }
            logger.LogDebug("backrun:{0} is end,startNb:{1} endNb:{2}", _context.Option.BrunType,_context.startNb, _context.endNb);
            return Task.CompletedTask;
        }
    }
}
