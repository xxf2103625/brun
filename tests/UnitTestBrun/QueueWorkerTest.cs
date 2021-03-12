using Brun;
using BrunTestHelper.QueueBackRuns;
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
    //TODO QueueWorker单元测试
    [TestClass]
    public class QueueWorkerTest : BaseQueueHostTest
    {
        [TestMethod]
        public async Task TestExcept()
        {
            string key = nameof(TestExcept);
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    
                    IQueueWorker worker = WorkerBuilder
                        .CreateQueue<LogQueueBackRun>()
                        .AddQueue<ErrorQueueBackRun>()
                        .SetKey(key)
                        .Build()
                        .AsQueueWorker()
                        ;
                    services.AddBrunService();
                })
                .Build();
            await host.StartAsync(cancellationToken);

            IQueueWorker worker = WorkerServer.Instance.GetQueueWorker(key);
            for (int i = 0; i < 100; i++)
            {
                worker.Enqueue($"测试消息:{i}");
                worker.Enqueue<ErrorQueueBackRun>($"内部异常{i}");
            }
            await WaitForBackRun();

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
