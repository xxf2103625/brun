using Brun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.QueueBackRuns
{
    public class ErrorQueueBackRun : QueueBackRun
    {
        public override Task Run(string message, CancellationToken stoppingToken)
        {
            throw new NotImplementedException("未实现");
            return Task.CompletedTask;
        }
    }
}
