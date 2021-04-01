using Brun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrunWebTest
{
    public class LogBackRun : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("LogBackRun log");
            return Task.CompletedTask;
        }
    }
    public class LongTimeBackRun : BackRun
    {
        public override async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            Console.WriteLine("LongTimeBackRun is runed");
        }
    }
}
