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
        private object SynchroRun_LOCK = new object();
        public SynchroBeforRunObserver() : base(Enums.WorkerEvents.StartRun, 5)
        {

        }
        public override async Task Todo(WorkerContext _context)
        {
            //TODO 优化同步逻辑
            while (_context.Tasks.Any(m => m.Status == TaskStatus.WaitingForActivation))
            {
                await Task.Delay(5);
            }
        }
    }
}
