﻿using Brun;
using BrunTestHelper.BackRuns;
using BrunTestHelper.WorkerObservers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
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
                    options.ConfigreWorkerServer = workerServer =>
                    {
                        var config = new WorkerConfig(key, "name");
                        config.AddWorkerObserver(new List<Brun.Observers.WorkerObserver>()
                        {
                        new TestStartWorkerObserver_20(),
                            new TestStartWorkerObserver_30()
                        });
                        var data=new ConcurrentDictionary<string, string>();
                        data.TryAdd("Order", "0");
                        workerServer.CreateOnceWorker(config).SetData(data).AddBrun<LogBackRun>();
                    };
                });
            });
            //var workerService=(Brun.Services.IOnceWorkerService) host.Services.GetService(typeof(Brun.Services.IWorkerService));
            IOnceWorker worker = (IOnceWorker)GetWorkerByKey(key);
            //Brun.BaskRuns.IBackRun worker =onceWorkerService.().First(m => m.Key == key).Value;
            worker.Run();
            WaitForBackRun(1);
            Assert.AreEqual("30", worker.GetData()["Order"]);
        }
    }
}
