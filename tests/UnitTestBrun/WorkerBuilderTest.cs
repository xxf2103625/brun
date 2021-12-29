using Brun;
using BrunTestHelper.BackRuns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    [TestClass]
    public class WorkerBuilderTest : BaseHostTest
    {
        [TestMethod]
        public void TestCreateAsync()
        {
            string key = Guid.NewGuid().ToString();
            string name = "run_1";
            //string tag = "myTag";
            StartHost(m =>
            {
                m.AddBrunService(options =>
                {
                    options.ConfigreWorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig(key, name))
                            .AddBrun(typeof(SimpleBackRun),new Brun.Options.OnceBackRunOption());
                    };
                });
                // WorkerBuilder.Create<SimpleBackRun>()
                //.SetNameTagKey(name, tag, key)
                //.Build();
            });
            
            IWorker work = GetWorkerByKey(key);
            Assert.AreEqual(key, work.Key);
            Assert.AreEqual(name, work.Name);
        }
        [TestMethod]
        public void TestEmptyCreateAsync()
        {
            StartHost(m =>
            {
                m.AddBrunService(options =>
                {
                    options.ConfigreWorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig())
                        .AddBrun(typeof(SimpleBackRun),new Brun.Options.OnceBackRunOption());
                    };
                });
                //WorkerBuilder.Create<SimpleBackRun>()
                //.Build();
            });
            IWorker work = GetWorkerByName(nameof(Brun.Workers.OnceWorker)).First();
            Assert.IsNotNull(work.Key);
            Assert.AreEqual("OnceWorker", work.Name);
        }
    }
}
