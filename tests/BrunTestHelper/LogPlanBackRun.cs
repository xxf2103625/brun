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
    public class LogPlanBackRun : PlanBackRun
    {
        public LogPlanBackRun(PlanBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("{1},LogPlanBackRun just console, Thread Id:{0}", Thread.CurrentThread.ManagedThreadId,DateTime.Now);
            return Task.CompletedTask;
        }
    }
}
