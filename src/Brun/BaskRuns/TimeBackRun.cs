using Brun.BaskRuns;
using Brun.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    public abstract class TimeBackRun : BackRun, IRun
    {
        public TimeBackRun(TimeBackRunOption option) : base(option)
        {

        }
        public TimeBackRunOption Option => (TimeBackRunOption)option;
        public abstract Task Run(CancellationToken stoppingToken);
    }
}
