using Brun;
using Brun.BaskRuns;
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
}
