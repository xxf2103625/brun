using Brun;
using Brun.Options;
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
            StartHost(services =>
            {
                string key = nameof(TestExcept);
                services.AddBrunService(options =>
                {
                    options.ConfigreWorkerServer =  workerServer =>
                    {
                        var wk =  workerServer.CreateQueueWorker(new WorkerConfig(key, "name")).AddBrun(typeof(LogQueueBackRun), new QueueBackRunOption());
                        wk.AddBrun(typeof(ErrorQueueBackRun), new QueueBackRunOption());
                    };
                });
            });
            IQueueWorker worker = (IQueueWorker)GetWorkerByKey(key);// GetQueueWorker(key);
            //worker.Start();
            for (int i = 0; i < 100; i++)
            {
                worker.Enqueue<LogQueueBackRun>($"测试消息:{i}");
                worker.Enqueue<ErrorQueueBackRun>($"内部异常{i}");
            }
            Assert.AreNotEqual(200, worker.Context.startNb);
            Assert.AreNotEqual(100, worker.Context.exceptNb);
            Assert.AreNotEqual(200, worker.Context.endNb);
            Console.WriteLine("wait before start:{0},except:{1},end:{2}", worker.Context.startNb, worker.Context.exceptNb, worker.Context.endNb);
            WaitForBackRun(200);
            Assert.AreEqual(0, worker.Context.RunningTasks.Count);
            Assert.AreEqual(200, worker.Context.startNb);
            Assert.AreEqual(100, worker.Context.exceptNb);
            Assert.AreEqual(200, worker.Context.endNb);
        }
        [TestMethod]
        public void TestStartAndStopAsync()
        {
            string key = nameof(TestStartAndStopAsync);
            StartHost(services =>
            {
                string key = nameof(TestStartAndStopAsync);
                services.AddBrunService(options =>
                {
                    options.ConfigreWorkerServer =  workerServer =>
                    {
                        var wk= workerServer.CreateQueueWorker(new WorkerConfig(key, "name")).AddBrun(typeof(LogQueueBackRun), new QueueBackRunOption());
                         wk.AddBrun(typeof(ErrorQueueBackRun), new QueueBackRunOption());
                    };
                });
                //WorkerBuilder
                //   .CreateQueue<LogQueueBackRun>()
                //   .AddQueue<ErrorQueueBackRun>()
                //   .SetKey(key)
                //   .Build<QueueWorker>()
                //   ;
                //services.AddBrunService();
            });
            var worker = (IQueueWorker)GetWorkerByKey(key);
            for (int i = 0; i < 1; i++)
            {
                worker.Enqueue($"测试消息:{i}");
            }
            WaitForBackRun();
            Assert.AreEqual(1, worker.Context.endNb);

            worker.Stop();

            for (int i = 1; i < 11; i++)
            {
                worker.Enqueue($"测试消息:{i}");
            }
            WaitForBackRun();
            Assert.AreEqual(1, worker.Context.startNb);
            Assert.AreEqual(1, worker.Context.endNb);

            worker.Start();
            WaitForBackRun(11);
            Assert.AreEqual(11, worker.Context.startNb);
            Assert.AreEqual(11, worker.Context.endNb);
        }
    }

}
