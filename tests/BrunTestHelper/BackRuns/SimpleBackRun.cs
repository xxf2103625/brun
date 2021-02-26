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
    public class SimpleBackRun : IBackRun
    {
        public async Task Run(CancellationToken stoppingToken)
        {

            await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);

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
