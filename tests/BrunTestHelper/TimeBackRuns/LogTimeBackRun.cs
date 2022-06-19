using Brun;
using Brun.BaskRuns;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper
{
    public class LogTimeBackRun : TimeBackRun
    {
        public LogTimeBackRun(TimeBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("LogTimeBackRun just console, Thread Id:{0}", Thread.CurrentThread.ManagedThreadId);
            return Task.CompletedTask;
        }
    }
    public class TimeErrorBackRun : TimeBackRun
    {
        public TimeErrorBackRun(TimeBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("LogTimeBackRun just console, Thread Id:{0}", Thread.CurrentThread.ManagedThreadId);
            throw new Exception("TimeErrorBackRun error test");
            //return Task.CompletedTask;
        }
    }
    public class LogTimeLongBackRun : TimeBackRun
    {
        public LogTimeLongBackRun(TimeBackRunOption option) : base(option)
        {
        }

        public override async Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("LogTimeBackRun begin, Thread Id:{0}", Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(12000);
            Console.WriteLine("LogTimeBackRun end, Thread Id:{0}", Thread.CurrentThread.ManagedThreadId);
            //return Task.CompletedTask;
        }
    }
}
