using Brun.Contexts;
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
            Console.WriteLine("SynchroBeforRunObserver,start:{0},end:{1}", brunContext.StartNb, _context.endNb);
            if (brunContext.StartNb > _context.endNb)
            {
                Thread.Sleep(5);
            }
            return Task.CompletedTask;
        }
    }
}
