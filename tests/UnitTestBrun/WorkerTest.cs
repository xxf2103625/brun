using Brun;
using BrunTestHelper.BackRuns;
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
    public class WorkerTest : BaseHostTest
    {
        [TestMethod]
        public Task TestSimpleRun()
        {
            IWorker work = WorkerBuilder.Create<SimpleNumberRun>()
                .Build();
            work.Run();
            //不await结果会直接先运行下面代码
            Console.WriteLine($"nb:{SimpleNumberRun.Nb}");
            Assert.AreNotEqual(100, SimpleNumberRun.Nb);
            return Task.CompletedTask;
        }
        [TestMethod]
        public async Task TestSimpleRunAsync()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["nb"] = 0;
            IWorker work = WorkerBuilder.Create<SimpleNumberRun>()
                .SetData(data)
                .Build();
            await work.Run();
            //await结果会卡主这个线程,下面包装整await后的回调
            Console.WriteLine($"nb:{work.GetData("nb")}");
            Assert.AreEqual(100, work.GetData("nb"));
        }
        [TestMethod]
        public async Task TaskTestErrorRun()
        {
            IWorker work = WorkerBuilder.Create<ErrorBackRun>()
               .Build();
            await work.Run();
            Assert.AreEqual(1, work.Context.exceptNb);
            Assert.AreEqual(typeof(NotImplementedException), work.Context.Exceptions.First().GetType());
            Assert.AreEqual("测试异常", work.Context.Exceptions.First().Message);
            //Assert.ThrowsException<NotImplementedException>(async () =>await work.Run(), "测试异常");
            //return Task.CompletedTask;
        }
        [TestMethod]
        public async Task SynchroWorkerTest()
        {
            IWorker worker = WorkerBuilder.Create<SimpleBackRun>().SetWorkerType(typeof(SynchroWorker))
                .Build();
            await worker.Run();
        }
        [TestMethod]
        public async Task SynchroWorkerManyTest()
        {
            IWorker worker = WorkerBuilder.Create<SimpeManyBackRun>().SetWorkerType(typeof(SynchroWorker))
                .Build();
            await worker.Run();
        }
        [TestMethod]
        public async Task CuntomDataTest()
        {
            var data = new Dictionary<string, object>();
            data["nb"] = 0;
            IWorker worker = WorkerBuilder.Create<CuntomDataBackRun>()
                .SetData(data)
                .Build();
            for (int i = 0; i < 10; i++)
            {
                await worker.Run();
            }
            int nb= worker.GetData<int>("nb");
            Assert.AreEqual(10, nb);
        }
    }
}
