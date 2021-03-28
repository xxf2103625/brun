using Brun;
using BrunTestHelper.BackRuns;
using BrunTestHelper.WorkerObservers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun.WorkerObservers
{
    [TestClass]
    public class ObserverOrderTest : BaseHostTest
    {
        [TestMethod]
        public void Test()
        {
            string key = nameof(Test);
            StartHost(services =>
            {
                string key = nameof(Test);
                WorkerBuilder.Create<LogBackRun>()
                .SetKey(key)
                .SetConfig(configure =>
                {
                    configure.AddWorkerObserver(new TestStartWorkerObserver_20());
                    configure.AddWorkerObserver(new TestStartWorkerObserver_30());
                })
                .Build();
            });
            IOnceWorker worker = WorkerServer.Instance.GetOnceWorker(key);
            worker.Run();
            WaitForBackRun();
            Assert.AreEqual("30", worker.Context.Items["Order"]);
        }
    }
}
