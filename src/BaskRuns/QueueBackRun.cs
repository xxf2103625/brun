using Brun.BaskRuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    public abstract class QueueBackRun : BackRunServicePrivoder, IQueueBackRun
    {
        public abstract Task Run(string message, CancellationToken stoppingToken);
    }
}
