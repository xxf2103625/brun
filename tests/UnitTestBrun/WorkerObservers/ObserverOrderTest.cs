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
                services.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        var config = new WorkerConfig(key, "name");
                        config.AddWorkerObserver(new List<Brun.Observers.WorkerObserver>()
                        {
                        new TestStartWorkerObserver_20(),new TestStartWorkerObserver_30()
                        });
                        workerServer.CreateOnceWorker(config).AddBrun<LogBackRun>();
                    };
                });
            });
            var onceWorkerService=(Brun.Services.IOnceWorkerService) host.Services.GetService(typeof(Brun.Services.IOnceWorkerService));
            IOnceWorker worker = WorkerServer.Instance.GetOnceWorker(key);
            //Brun.BaskRuns.IBackRun worker =onceWorkerService.().First(m => m.Key == key).Value;
            worker.Run();
            WaitForBackRun();
            Assert.AreEqual("30", worker.GetData()["Order"]);
        }
    }
}
