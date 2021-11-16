﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    public abstract class TimeBackRun : BackRun
    {
        private TimeBackRunOption _option;
        public TimeBackRun(TimeBackRunOption option)
        {
            this._option = option;
        }
        public override string Id => _option.Id;
        public TimeBackRunOption Option => _option;
        //public abstract Task Run(CancellationToken stoppingToken);
        public void SetTimeBackRun(TimeBackRunOption option)
        {
            this._option = option;
        }
    }
}