using Brun;
using Brun.BaskRuns;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class AwaitErrorBackRun : OnceBackRun
    {
        public AwaitErrorBackRun(OnceBackRunOption option) : base(option)
        {
        }

        public override async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(3),stoppingToken);
            throw new NotImplementedException("测试异常");
        }
     
    }
}
