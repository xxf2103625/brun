using Brun;
using Brun.Workers;
using BrunTestHelper.BackRuns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    [TestClass]
    public class WorkerTest : BaseHostTest
    {
        [TestMethod]
        public Task TestSimpleRun()
        {
            IOnceWorker work = WorkerBuilder.Create<SimpleNumberRun>()
                .BuildOnceWorker();

            work.RunDontWait();
            //不await结果会直接先运行下面代码
            Console.WriteLine($"nb:{SimpleNumberRun.Nb}");
            Assert.AreEqual(100, SimpleNumberRun.Nb);
            return Task.CompletedTask;
        }
        [TestMethod]
        public async Task TestSimpleRunAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpleNumberRun>()
                .SetData(data)
                .BuildOnceWorker();
            await work.Run();
            //await结果会卡主这个线程,下面包装整await后的回调
            Console.WriteLine($"nb:{work.GetData("nb")}");
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TaskTestErrorRunNotAwait()
        {
            IOnceWorker work = WorkerBuilder.Create<ErrorBackRun>()
               .BuildOnceWorker();
            for (int i = 0; i < 10; i++)
            {
                work.RunDontWait();
            }
            //任务不等待主线程
            Assert.AreEqual(0, work.Context.exceptNb);
        }
        [TestMethod]
        public async Task TaskTestErrorRunAwaitAsync()
        {
            IOnceWorker work = WorkerBuilder.Create<ErrorBackRun>()
               .BuildOnceWorker();
            await work.Run();
            //任务等待主线程
            Assert.AreEqual(1, work.Context.exceptNb);
            Assert.AreEqual(typeof(NotImplementedException), work.Context.Exceptions.First().GetType());
            Assert.AreEqual("测试异常", work.Context.Exceptions.First().Message);
        }
        [TestMethod]
        public async Task SynchroWorkerTest()
        {
            IOnceWorker worker = WorkerBuilder.Create<SimpleBackRun>().SetWorkerType(typeof(SynchroWorker))
                .BuildOnceWorker();
            for (int i = 0; i < 3; i++)
            {
                await worker.Run();
            }
        }
        [TestMethod]
        public async Task SynchroWorkerManyTest()
        {
            IOnceWorker worker = WorkerBuilder.Create<SimpeManyBackRun>().SetWorkerType(typeof(SynchroWorker))
                .BuildOnceWorker();
            for (int i = 0; i < 3; i++)
            {
                await worker.Run();
            }
        }
        [TestMethod]
        public void SynchroWorkerManyDontWaitTest()
        {
            IOnceWorker worker = WorkerBuilder.Create<SimpeManyBackRun>().SetWorkerType(typeof(SynchroWorker))
                .BuildOnceWorker();
            for (int i = 0; i < 3; i++)
            {
                worker.RunDontWait();
            }
            //测试进程会直接结束
        }
        [TestMethod]
        public async Task CuntomDataTest()
        {
            var data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker worker = WorkerBuilder.Create<CuntomDataBackRun>()
                .SetData(data)
                .BuildOnceWorker();
            for (int i = 0; i < 10; i++)
            {
                await worker.Run();
            }
            string nb = worker.GetData("nb");
            Assert.AreEqual("10", nb);
        }
    }
}
