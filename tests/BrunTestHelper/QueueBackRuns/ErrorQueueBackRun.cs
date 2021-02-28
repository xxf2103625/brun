using Brun;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.QueueBackRuns
{
    public class LogQueueBackRun : QueueBackRun
    {
        public override Task Run(string message, CancellationToken stoppingToken)
        {
            var log= GetService<ILogger<LogQueueBackRun>>();
            Console.WriteLine("接收到消息：{0}",message);
            log.LogInformation("接收到消息:{0}", message);
            return Task.CompletedTask;
        }
    }
    public class ErrorQueueBackRun : QueueBackRun
    {
        public override Task Run(string message, CancellationToken stoppingToken)
        {
            throw new NotImplementedException("未实现");
        }
    }
}
