using Brun;
using Microsoft.Extensions.Logging;
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
            //string nb = Data["nb"];
            //nb = (int.Parse(nb) + 1).ToString();
            lock (SharedLock.Nb_LOCK)
            {
                Data["nb"] = (int.Parse(Data["nb"]) + 1).ToString();
                Console.WriteLine($"nb:{Data["nb"]}");
            }
            GetRequiredService<ILogger<CuntomDataBackRun>>().LogInformation("Thread.Id:" + Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }
    }
}
