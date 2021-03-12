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
    //TODO 重构方便测试子线程的流程
    /// <summary>
    /// 测试Host托管的后台任务基类
    /// </summary>
    public abstract class BaseHostTest
    {
        protected IHost host;
        CancellationToken cancellationToken;
        CancellationTokenSource tokenSource;
        //TimeSpan waitTime = TimeSpan.FromSeconds(3);

        [TestInitialize]
        public async Task InitAsync()
        {
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddBrunService();
                })
                .Build();
            await host.StartAsync(cancellationToken);
            //WorkerServer.Instance.Start(host.Services, cancellationToken);
        }
        protected async Task WaitForBackRun()
        {
            WorkerServer server = (WorkerServer)host.Services.GetRequiredService<IWorkerServer>();
            while (server.GetAllWorker().Any(m => m.RunningTasks.Count > 0))
            {
                await Task.Delay(5);
            }
        }
        [TestCleanup]
        public async Task CleanupAsync()
        {
            //await Task.Delay(waitTime);
            //tokenSource.CancelAfter(waitTime);
            tokenSource.Cancel();
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
