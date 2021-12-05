using Brun;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class AwaitErrorBackRun : BackRun
    {
        public AwaitErrorBackRun(BackRunOption option) : base(option)
        {
        }

        public override async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(3),stoppingToken);
            throw new NotImplementedException("测试异常");
        }
     
    }
}
