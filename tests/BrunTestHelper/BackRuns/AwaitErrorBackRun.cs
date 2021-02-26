using Brun;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class AwaitErrorBackRun : IBackRun
    {
        public async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(3));
            throw new NotImplementedException("测试异常");
        }
    }
}
