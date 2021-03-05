using Brun;
using BrunTestHelper.BackRuns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    public abstract class BaseHostTest
    {
        protected IHost host;
        CancellationToken cancellationToken;
        CancellationTokenSource tokenSource;
        TimeSpan waitTime = TimeSpan.FromSeconds(3);
        [TestInitialize]
        public async Task InitAsync()
        {
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    //services.AddSingleton<SimpleNumberRun>();
                    services.AddBrunService();
                })
                .Build();
            await host.StartAsync(cancellationToken);
            //WorkerServer.Instance.Start(host.Services, cancellationToken);
        }
        [TestCleanup]
        public async Task CleanupAsync()
        {
            //await Task.Delay(waitTime);
            tokenSource.CancelAfter(waitTime);

            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(5);
            }
            if (host != null)
            {
                await host.StopAsync();
            }
        }
    }
}
