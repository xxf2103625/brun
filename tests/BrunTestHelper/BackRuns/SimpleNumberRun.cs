﻿using Brun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class SimpleNumberRun : BackRun
    {
        public static int Nb = 0;
        public override Task Run(CancellationToken stoppingToken)
        {
            for (int i = 0; i < 100; i++)
            {
                Nb++;
                Console.WriteLine($"SimpleNumberRun.Nb:{Nb}");
                Data["nb"] = (int)Data["nb"] + 1;
                //Thread.Sleep(TimeSpan.FromSeconds(0.1));
                //await Task.Delay(TimeSpan.FromSeconds(0.1));
            }
            return Task.CompletedTask;
        }
    }
}