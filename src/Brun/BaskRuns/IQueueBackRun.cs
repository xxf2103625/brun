using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    public interface IQueueBackRun
    {
        Task Run(string message, CancellationToken stoppingToken);
    }
}
