using Brun.Commons;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Observers
{
    public class WorkerStartRunObserver : WorkerObserver
    {
        private static object nb_LOCK = new object();
        private ILogger<WorkerStartRunObserver> logger;
        public WorkerStartRunObserver() : base(Enums.WorkerEvents.StartRun, 10)
        {
            
        }

        public override Task Todo(WorkerContext _context, Type brunType)
        {
            logger = _context.ServiceProvider.GetRequiredService<ILogger<WorkerStartRunObserver>>();
            lock (nb_LOCK)
            {
                _context.startNb++;
            }
            logger.LogDebug("backrun:{0} is start,startNb:{1} {2}", brunType.Name, _context.startNb, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss FFFF"));
            return Task.CompletedTask;
        }
    }
}
