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
        //private object SynchroRun_LOCK = new object();
        public SynchroBeforRunObserver() : base(Enums.WorkerEvents.StartRun, 5)
        {

        }
        private static object Sync_LOCK = new object();
        private int initNb = -1;
        public override Task Todo(WorkerContext _context, BrunContext brunContext)
        {
            Console.WriteLine("SynchroBeforRunObserver,start:{0},end:{1}", _context.startNb, _context.endNb);
            //TODO 移到Worker直接控制StartNew
            //if (_context.startNb > _context.endNb)
            //{
            //    lock (Sync_LOCK)
            //    {
            //        Console.WriteLine("SynchroBeforRunObserver before,start:{0},end:{1}", _context.startNb, _context.endNb);
            //        if (_context.startNb > _context.endNb)
            //        {
            //            while (_context.startNb > _context.endNb)
            //            {
            //                Thread.Sleep(50);
            //            }
            //        }
            //        Console.WriteLine("SynchroBeforRunObserver next,start:{0},end:{1}", _context.startNb, _context.endNb);
            //    }
            //}
            return Task.CompletedTask;
        }
    }
}
