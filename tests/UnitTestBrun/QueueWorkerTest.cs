using Brun;
using Brun.Workers;
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
    [TestClass]
    public class QueueWorkerTest : BaseHostTest
    {
        [TestMethod]
        public void TestExcept()
        {
            string key = nameof(TestExcept);
            StartHostAsync(services =>
            {
                string key = nameof(TestExcept);
                IQueueWorker worker = WorkerBuilder
                        .CreateQueue<LogQueueBackRun>()
                        .AddQueue<ErrorQueueBackRun>()
                        .SetKey(key)
                        .Build()
                        .AsQueueWorker()
                        ;
                services.AddBrunService();
            });
            IQueueWorker worker = WorkerServer.Instance.GetQueueWorker(key);
            for (int i = 0; i < 100; i++)
            {
                worker.Enqueue($"测试消息:{i}");
                worker.Enqueue<ErrorQueueBackRun>($"内部异常{i}");
            }
            Assert.AreNotEqual(200, worker.Context.startNb);
            Assert.AreNotEqual(100, worker.Context.exceptNb);
            Assert.AreNotEqual(200, worker.Context.endNb);
            Console.WriteLine("wait before start:{0},except:{1},end:{2}", worker.Context.startNb, worker.Context.exceptNb, worker.Context.endNb);
            WaitForBackRun();
            Assert.AreEqual(0, worker.RunningTasks.Count);
            Assert.AreEqual(200, worker.Context.startNb);
            Assert.AreEqual(100, worker.Context.exceptNb);
            Assert.AreEqual(200, worker.Context.endNb);
        }
        [TestMethod]
        public async Task TestStartAndStopAsync()
        {
            string key = nameof(TestStartAndStopAsync);
            StartHostAsync(services =>
            {
                string key = nameof(TestStartAndStopAsync);
                     WorkerBuilder
                        .CreateQueue<LogQueueBackRun>()
                        .AddQueue<ErrorQueueBackRun>()
                        .SetKey(key)
                        .Build()
                        .AsQueueWorker()
                        ;
                services.AddBrunService();
            });
            IQueueWorker worker = WorkerServer.Instance.GetQueueWorker(key);
            for (int i = 0; i < 1; i++)
            {
                worker.Enqueue($"测试消息:{i}");
            }
            WaitForBackRun();
            Assert.AreEqual(1, worker.Context.endNb);

            await worker.Stop();

            for (int i = 1; i < 11; i++)
            {
                worker.Enqueue($"测试消息:{i}");
            }
            WaitForBackRun();
            Assert.AreEqual(1, worker.Context.startNb);
            Assert.AreEqual(1, worker.Context.endNb);

            await worker.Start();
            WaitForBackRun();
            Assert.AreEqual(11, worker.Context.startNb);
            Assert.AreEqual(11, worker.Context.endNb);
        }
    }
    
}
