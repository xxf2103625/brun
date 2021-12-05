using Brun;
using Brun.Options;
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

        public SimpleBackRun(BackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            //SimNb++;
            lock (SharedLock.Nb_LOCK)
            {
                SimNb++;
                Console.WriteLine($"SimNb:{SimNb}");
            }
            return Task.CompletedTask;
        }
    }
    public class SimpleLongBackRun : BackRun
    {
        public static int SimNb = 0;

        public SimpleLongBackRun(BackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            //SimNb++;
            lock (SharedLock.Nb_LOCK)
            {
                SimNb++;
                Console.WriteLine($"SimNb:{SimNb}");
            }
            return Task.CompletedTask;
        }
    }
    public class SimpeManyBackRun : BackRun
    {
        public SimpeManyBackRun(BackRunOption option) : base(option)
        {
        }

        public override async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            var log = GetRequiredService<ILogger<SimpeManyBackRun>>();
            for (int i = 0; i < 3; i++)
            {
                log.LogInformation($"第 {i} 次运行");
                await Task.Delay(TimeSpan.FromSeconds(0.1), stoppingToken);
            }
        }
    }
}
