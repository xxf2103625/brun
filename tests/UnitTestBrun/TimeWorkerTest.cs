using Brun;
using BrunTestHelper.BackRuns;
using BrunTestHelper.QueueBackRuns;
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
    public class TimeWorkerTest : BaseHostTest
    {
        [TestMethod]
        public void TestDateTime()
        {
            StartHost(services =>
            {

            });
            DateTimeOffset offset1 = DateTime.Now;
            DateTimeOffset offset2 = DateTime.UtcNow;
            Console.WriteLine("off1:{0},off2:{1}", offset1.ToString(), offset2.ToString());
            Assert.AreNotEqual(offset1.ToLocalTime(), offset2.ToLocalTime());
        }
        [TestMethod]
        public void TestTimeWorker()
        {
            int sleepTime = 2;
            string key = nameof(TestTimeWorker);
            StartHost(services =>
            {
                string key = nameof(TestTimeWorker);
                services.AddBrunService(workerServer =>
                {
                    workerServer.CreateTimeWorker(new WorkerConfig(key, "")).AddBrun(typeof(BrunTestHelper.LogTimeBackRun), new TimeBackRunOption(TimeSpan.FromSeconds(sleepTime)));
                });

                //WorkerBuilder
                //   .CreateTime<LogBackRun>(TimeSpan.FromSeconds(sleepTime), false)
                //   .SetKey(key)
                //   .Build()
                //   ;
                //services.AddBrunService();
            });
            IWorker woker = WorkerServer.Instance.GetWorker(key);
            Assert.AreEqual(0, woker.Context.startNb);
            Assert.AreEqual(0, woker.Context.exceptNb);
            Assert.AreEqual(0, woker.Context.endNb);
            Console.WriteLine("Main Thread id:{0}", Thread.CurrentThread.ManagedThreadId);
            WiatAfter(TimeSpan.FromSeconds(2.5));
            Assert.AreEqual(1, woker.Context.startNb);
            Assert.AreEqual(0, woker.Context.exceptNb);
            Assert.AreEqual(1, woker.Context.endNb);

        }
    }
}
