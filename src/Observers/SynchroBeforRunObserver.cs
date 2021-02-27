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
        private static object SynchroRun_LOCK = new object();
        public SynchroBeforRunObserver() : base(Enums.WorkerEvents.StartRun, 5)
        {

        }
        public override Task Todo(WorkerContext _context)
        {
            if (_context.endNb < _context.startNb)
            {
                lock (SynchroRun_LOCK)
                {
                    while (_context.endNb < _context.startNb && !WorkerServer.Instance.StoppingToken.IsCancellationRequested)
                    {
                        Thread.Sleep(5);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
