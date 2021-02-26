using Brun;
using Brun.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class LogBackRun : IBackRun
    {
        public Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("LogBackRun just console");
            return Task.CompletedTask;
        }
    }
}
