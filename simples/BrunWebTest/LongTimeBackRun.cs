using Brun;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrunWebTest
{
    public class LogBackRun : OnceBackRun
    {
        public LogBackRun(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("LogBackRun log");
            return Task.CompletedTask;
        }
    }
    public class LongTimeBackRun : OnceBackRun
    {
        public LongTimeBackRun(OnceBackRunOption option) : base(option)
        {
        }

        public override async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            Console.WriteLine("LongTimeBackRun is runed");
        }
    }
}
