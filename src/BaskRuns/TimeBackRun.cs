using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    public abstract class TimeBackRun
    {
        public Task Run(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
