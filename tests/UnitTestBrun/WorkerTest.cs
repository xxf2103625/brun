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
    /*
     * 结论
     *BackRun内部有await ：await work.Run()会回到主线程。 work.Run()：调用线程直接往后执行。
     *BackRun内部返回Task.CompletedTask：不论await work.Run(),还是work.Run()，调用线程都会等待
     *work.RunDontWait()，强制线程池执行，并丢弃子线程信息，主线程无法捕捉
     *当BackRun中有await时，使用work.Run() 才和RunDontWait()效果一样
     */
    [TestClass]
    public class WorkerTest : BaseHostTest
    {
        [TestMethod]
        public void TestSimpleRunDontWait()
        {
            IOnceWorker work = WorkerBuilder
                .Create<SimpleNumberRun>()//内部没有await
                .BuildOnceWorker();
            work.RunDontWait();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //不会等待
            Assert.AreEqual(null, work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRun()
        {
            IOnceWorker work = WorkerBuilder
                .Create<SimpleNumberRun>()//内部没有await
                .BuildOnceWorker();
            await work.Run();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //等待
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public Task TestSimpleRunAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBefore>()//内部有await
                .SetData(data)
                .BuildOnceWorker();
            work.Run();
            //不等待 //backrun内部用了await
            Assert.AreEqual("0", work.GetData("nb"));
            return Task.CompletedTask;
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBefore>()// 内部有await
                .SetData(data)
                .BuildOnceWorker();
            await work.Run();
            //等待结果
            Console.WriteLine("UI线程结束");
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeAsyncAwait()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBefore>()// 内部await
                .SetData(data)
                .BuildOnceWorker();
            await work.Run();
            //等待结果
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskWaitWorkerVoid()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBeforeTask>() //内部没有await
                .SetData(data)
                .BuildOnceWorker();
            work.Run();
            //会等待， 即时work.Run()不加await， BackRun内部是同步方法也会等待
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeTaskWaitWorkerAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBeforeTask>() //内部没有await
                .SetData(data)
                .BuildOnceWorker();
            await work.Run();
            //会等待， 即时work.Run()不加await， BackRun内部是同步方法也会等待
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeVoid()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBefore>()//run内使用了async
                .SetData(data)
                .BuildOnceWorker();
            work.Run();
            //不会等待任务
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestAsyncSimpleRunBefore()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBefore>()//run内使用了async
                .SetData(data)
                .BuildOnceWorker();
            work.Run();
            //不等待任务
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskVoid()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBeforeTask>() //return Task.CompletedTask;
                .SetData(data)
                .BuildOnceWorker();
            work.Run();
            //等待任务
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeTaskAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["nb"] = "0";
            IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBeforeTask>() //return Task.CompletedTask;
                .SetData(data)
                .BuildOnceWorker();
            await work.Run();
            //等待任务
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TaskTestErrorRunNotVoidAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["b"] = "2";
            IOnceWorker work = WorkerBuilder.Create<ErrorBackRun>() //await
                .SetData(data)
               .BuildOnceWorker();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            //主线程不等待任务
            Assert.AreEqual(null, work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(0, work.Context.exceptNb);
        }
        [TestMethod]
        public async Task TaskTestErrorRunNotAwaitAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["b"] = "2";
            IOnceWorker work = WorkerBuilder.Create<ErrorBackRun>()//await
                .SetData(data)
               .BuildOnceWorker();
            for (int i = 0; i < 10; i++)
            {
                await work.Run();
            }
            //等待
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public async Task TaskTestErrorRunNotVoid_2()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["b"] = "2";
            IOnceWorker work = WorkerBuilder.Create<ErrorBackRun>() //await
                .SetData(data)
               .BuildOnceWorker();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            //另一种等待
            await WaitForBackRun();
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public async Task WaitTaskTestErrorRunNotAwaitAsync()
        {
            ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
            data["b"] = "2";
            IOnceWorker work = WorkerBuilder.Create<ErrorBackRun>()
                .SetData(data)
               .BuildOnceWorker();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            await WaitForBackRun();
            //任务等待主线程
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public void TaskTestErrorRunAwaitAsync()
        {
            IOnceWorker work = WorkerBuilder.Create<ErrorBackRun>()
               .BuildOnceWorker();
            work.Run();
            //任务不会等待主线程
            Assert.AreEqual(0, work.Context.exceptNb);
        }
        public static int SyTest = 0;
        [TestMethod]
        public async Task SynchroWorkerTest()
        {
            int max = 10000;
            IOnceWorker worker = WorkerBuilder.Create<SimpleBackRun>().SetWorkerType(typeof(SynchroWorker))
                .BuildOnceWorker();
            for (int i = 0; i < max; i++)
            {
                worker.Run();
            }
            await WaitForBackRun();
            Assert.AreEqual(max, SimpleBackRun.SimNb);
        }
        [TestMethod]
        public async Task SynchroWorkerManyTest()
        {
            IOnceWorker worker = WorkerBuilder.Create<SimpeManyBackRun>().SetWorkerType(typeof(SynchroWorker))
                .BuildOnceWorker();
            for (int i = 0; i < 3; i++)
            {
                worker.Run();
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
                worker.Run();
            }
            await WaitForBackRun();
            string nb = worker.GetData("nb");
            Assert.AreEqual("10", nb);
        }


        //多个Backrun
        [TestMethod]
        public async Task TestSimpleMultAsync()
        {
            IOnceWorker work = WorkerBuilder
                .Create<SimpleNumberRun>()//内部没有await
                .Add<SimpNbDelayBefore>()
                .Add<SimpNbDelayAfter>()
                .BuildOnceWorker();
            work.RunDontWait();
            work.RunDontWait<SimpNbDelayBefore>();
            work.RunDontWait<SimpNbDelayAfter>();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //都不等待
            Assert.AreEqual(null, work.GetData("nb"));
            await WaitForBackRun();
            //完成之后
            Assert.AreEqual("300", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunDontWaitMultAsync()
        {
            IOnceWorker work = WorkerBuilder
                .Create<SimpleNumberRun>()//内部没有await
                .Add<SimpNbDelayBefore>()
                .Add<SimpNbDelayAfter>()
                .BuildOnceWorker();
            work.RunDontWait();
            work.Run<SimpNbDelayBefore>();
            work.Run<SimpNbDelayAfter>();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //不会等待第一个
            Assert.AreEqual("200", work.GetData("nb"));
            //第一个会在这个后面继续运行
            await WaitForBackRun();
            Assert.AreEqual("300", work.GetData("nb"));
        }

    }
}
