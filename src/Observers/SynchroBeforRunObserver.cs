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
        //private object SynchroRun_LOCK = new object();
        public SynchroBeforRunObserver() : base(Enums.WorkerEvents.StartRun, 5)
        {

        }
        public override Task Todo(WorkerContext _context, Type brunType)
        {
            //while (_context.endNb < _context.startNb)
            //{
            //    Thread.Sleep(5);
            //}
            //TODO 优化同步逻辑
            //while(_context.Tasks.Any(m=>m.Status== TaskStatus.Running))
            //{
            //    Thread.Sleep(5);
            //}
            Task.WaitAny(_context.Tasks.ToArray());
            return Task.CompletedTask;
            //while (_context.Tasks.Any(m => m.Status == TaskStatus.Running))
            //{
            //    await Task.Delay(5);
            //}
        }
    }
}
