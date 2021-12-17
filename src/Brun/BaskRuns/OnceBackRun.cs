using Brun.BaskRuns;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// OnceWorker专用的BackRun
    /// </summary>
    public abstract class OnceBackRun : BackRun, IRun
    {
        public OnceBackRun(OnceBackRunOption option) : base(option)
        {
        }

        public abstract Task Run(CancellationToken stoppingToken);
        public OnceBackRunOption Option => (OnceBackRunOption)option;
    }
}
