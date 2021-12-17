using Brun;
using Brun.Commons;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class LogBackRun : OnceBackRun
    {
        public LogBackRun(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("LogBackRun just console, Thread Id:{0}",Thread.CurrentThread.ManagedThreadId);
            return Task.CompletedTask;
        }
    }
}
