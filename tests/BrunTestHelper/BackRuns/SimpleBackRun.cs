using Brun;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class SimpleBackRun : BackRun
    {
        public static int SimNb = 0;
        private object LOCK = new object();
        public override Task Run(CancellationToken stoppingToken)
        {
            lock (LOCK)
            {
                SimNb++;
            }
            //await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);
            return Task.CompletedTask;
        }
    }
    public class SimpeManyBackRun : BackRun
    {
        public override async Task Run(CancellationToken stoppingToken)
        {
            var log = GetRequiredService<ILogger<SimpeManyBackRun>>();
            for (int i = 0; i < 10; i++)
            {
                log.LogInformation($"第 {i} 次运行");
                await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);
            }
        }
    }
}
