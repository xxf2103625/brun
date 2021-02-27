using Brun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class ErrorBackRun : BackRun
    {
        public override async Task Run(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5),stoppingToken);
            throw new NotImplementedException("测试异常");
            
            //return Task.CompletedTask;
        }
    }
}
