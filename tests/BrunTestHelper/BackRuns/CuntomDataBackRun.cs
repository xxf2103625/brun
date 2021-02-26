using Brun;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class CuntomDataBackRun : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            int nb = (int)Data["nb"];
            nb++;
            Data["nb"] = nb;
            return Task.CompletedTask;
        }
    }
}
