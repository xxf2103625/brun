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
            string nb = Data["nb"];
            nb = (int.Parse(nb) + 1).ToString();
            Data["nb"] = nb;
            return Task.CompletedTask;
        }
    }
}
