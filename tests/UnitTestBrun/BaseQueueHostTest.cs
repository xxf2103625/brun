﻿using Brun;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    public abstract class BaseQueueHostTest
    {
        public IHost host;
        public CancellationToken cancellationToken;
        public CancellationTokenSource tokenSource;
        protected async Task WaitForBackRun()
        {
            WorkerServer server = (WorkerServer)host.Services.GetRequiredService<IWorkerServer>();
            await Task.Delay(TimeSpan.FromSeconds(0.1));
            while (server.GetAllWorker().Any(m => m.RunningTasks.Count > 0))
            {
                await Task.Delay(5);
            }
        }
    }
}
