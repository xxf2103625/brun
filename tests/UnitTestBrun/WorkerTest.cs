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
     *work.RunDontWait()，强制线程池执行，并丢弃子线程信息，主线程都不会等待
     *当BackRun中有await时，使用work.Run()不要await 才和RunDontWait()效果一样
     */
    [TestClass]
    public class WorkerTest : BaseHostTest
    {
        [TestMethod]
        public void TestSimpleRunDontWaitAsync()
        {
            StartHostAsync(m =>
            {
                WorkerBuilder
               .Create<SimpleNumberRun>()//内部没有await
               .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpleNumberRun)).First();
            work.RunDontWait();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //不会等待
            Assert.AreEqual(null, work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRun()
        {
            StartHostAsync(m =>
            {
                WorkerBuilder
                .Create<SimpleNumberRun>()//内部没有await
                .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpleNumberRun)).First();
            await work.Run();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //等待
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));

        }
        [TestMethod]
        public void TestSimpleRunAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBefore>()//内部有await
                   .SetData(data)
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBefore)).First();
            work.RunDontWait();
            //不等待 //backrun内部用了await
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBefore>()// 内部有await
                   .SetData(data)
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBefore)).First();
            await work.Run();
            //等待结果
            WaitForBackRun();
            Console.WriteLine("UI线程结束");
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeAsyncAwait()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBefore>()// 内部await
               .SetData(data)
               .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBefore)).First();
            await work.Run();
            //不会等待结果
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskWaitWorkerVoidAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBeforeTask>() //内部没有await
                   .SetData(data)
                   .BuildOnceWorker();
            });

            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBeforeTask)).First();
            work.Run();
            //不会等待，
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeTaskWaitWorkerAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBeforeTask>() //内部没有await
                   .SetData(data)
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBeforeTask)).First();
            await work.Run();
            //不会等待， 
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeVoidAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBefore>()//run内使用了async
                   .SetData(data)
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBefore)).First();
            work.Run();
            //不会等待任务
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public void TestAsyncSimpleRunBefore()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBefore>()//run内使用了async
                    .SetData(data)
                    .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBefore)).First();
            work.Run();
            //不等待任务
            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskVoidAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<SimpNbDelayBeforeTask>() //return Task.CompletedTask;
                   .SetData(data)
                   //另一种方式 等待任务
                   .SetConfig(config =>
                   {
                       config.TimeWaitForBrun = TimeSpan.FromSeconds(5);
                   })
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBeforeTask)).First();
            work.RunDontWait();

            Assert.AreEqual("0", work.GetData("nb"));
        }
        [TestMethod]
        public async Task TestSimpleRunBeforeTaskAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBeforeTask>() //return Task.CompletedTask;
                    .SetData(data)
                    .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpNbDelayBeforeTask)).First();
            await work.Run();
            //等待任务
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestErrorNb()
        {
            int max = 100;
            StartHostAsync(m =>
            {
                WorkerBuilder.Create<ErrorBackRun3>() //await
                  .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(ErrorBackRun3)).First();
            for (int i = 0; i < max; i++)
            {
                work.RunDontWait();
            }
            Assert.AreNotEqual(max, work.Context.startNb);
            Assert.AreNotEqual(max, work.Context.exceptNb);
            Assert.AreNotEqual(max, work.Context.exceptNb);
            WaitForBackRun();
            Assert.AreEqual(max, work.Context.startNb);
            Assert.AreEqual(max, work.Context.exceptNb);
            Assert.AreEqual(max, work.Context.endNb);
        }
        [TestMethod]
        public async Task TaskTestErrorRunNotVoidAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                WorkerBuilder.Create<ErrorBackRun3>() //await
                   .SetData(data)
                  .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(ErrorBackRun3)).First();
            for (int i = 0; i < 10; i++)
            {
                await work.Run();
            }
            Assert.AreEqual(0, work.Context.exceptNb);
            Assert.AreEqual(0, work.Context.endNb);
            WaitForBackRun();
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public void TaskTestRunNotAwaitAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                WorkerBuilder.Create<DataBackRun>()//await
                    .SetData(data)
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(DataBackRun)).First();
            for (int i = 0; i < 10; i++)
            {
                work.RunDontWait();
            }
            Assert.AreNotEqual(10, work.Context.startNb);
            Assert.AreNotEqual(10, work.Context.endNb);
            //等待
            WaitForBackRun();
            Console.WriteLine(string.Join(",", work.GetData().Select(m => $"{m.Key}:{m.Value}")));
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(0, work.Context.exceptNb);
            Assert.AreEqual(10, work.Context.endNb);
            //return Task.CompletedTask;
        }
        [TestMethod]//异常会
        public void TaskTestErrorRunNotAwaitAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                WorkerBuilder.Create<DataBackRun>()//await
                    .SetData(data)
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(DataBackRun)).First();
            for (int i = 0; i < 10; i++)
            {
                work.RunDontWait();
            }
            //等待
            WaitForBackRun();
            Console.WriteLine(string.Join(",", work.GetData().Select(m => $"{m.Key}:{m.Value}")));
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(0, work.Context.exceptNb);
        }
        [TestMethod]
        public void TaskTestErrorRunNotVoid_2Async()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                WorkerBuilder.Create<ErrorLongBackRun>() //await
                   .SetData(data)
                   .SetConfig(m =>
                   {
                       m.TimeWaitForBrun = TimeSpan.FromSeconds(10);
                   })
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(ErrorLongBackRun)).First();
            for (int i = 0; i < 10; i++)
            {
                work.RunDontWait();
            }
            Assert.AreEqual(null, work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(0, work.Context.exceptNb);
            WaitForBackRun();
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public void WaitTaskTestErrorRunNotAwaitAsync()
        {
            StartHostAsync(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                WorkerBuilder.Create<ErrorBackRun4>()
                    .SetData(data)
                    .SetConfig(m =>
                    {
                        m.TimeWaitForBrun = TimeSpan.FromSeconds(5);
                    })
                   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(ErrorBackRun4)).First();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            WaitForBackRun();
            //任务等待主线程
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public void TaskTestErrorRunAwaitAsync()
        {
            StartHostAsync(m =>
            {
                WorkerBuilder.Create<ErrorBackRun>()
               .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(ErrorBackRun)).First();
            work.RunDontWait();
            //任务不会等待主线程
            Assert.AreEqual(0, work.Context.exceptNb);
        }
        public static int SyTest = 0;
        [TestMethod]
        public async Task SynchroWorkerTest()
        {
            SimpleBackRun.SimNb = 0;
            int max = 10;
            StartHostAsync(m =>
            {
                WorkerBuilder.Create<SimpleBackRun>()
                    .Build<SynchroWorker>();
                    //.Build();
            });
            IOnceWorker worker = GetOnceWorkerByName(nameof(SimpleBackRun)).First();
            for (int i = 0; i < max; i++)
            {
                await worker.Run();
            }
            WaitForBackRun(max);
            Assert.AreEqual(max, SimpleBackRun.SimNb);
        }
        [TestMethod]
        public void SynchroWorkerTestVoid()
        {
            SimpleBackRun.SimNb = 0;
            int max = 10;
            StartHostAsync(m =>
            {
                WorkerBuilder.Create<SimpleBackRun>()
                    .Build<SynchroWorker>();
            });
            IOnceWorker worker = GetOnceWorkerByName(nameof(SimpleBackRun)).First();
            for (int i = 0; i < max; i++)
            {
                worker.RunDontWait();
            }
            WaitForBackRun(max);
            Assert.AreEqual(max, SimpleBackRun.SimNb);
        }
        [TestMethod]
        public void SynchroWorkerManyTest()
        {
            StartHostAsync(m =>
            {
                WorkerBuilder.Create<SimpeManyBackRun>()
                             .Build<SynchroWorker>();
            });
            IOnceWorker worker = GetOnceWorkerByName(nameof(SimpeManyBackRun)).First();
            for (int i = 0; i < 3; i++)
            {
                worker.Run();
            }
            WaitForBackRun();
        }
        [TestMethod]
        public void SynchroWorkerManyDontWaitTestAsync()
        {
            StartHostAsync(m =>
            {
                WorkerBuilder.Create<SimpeManyBackRun>().SetWorkerType(typeof(SynchroWorker))
               .BuildOnceWorker();
            });
            IOnceWorker worker = GetOnceWorkerByName(nameof(SimpeManyBackRun)).First();
            for (int i = 0; i < 3; i++)
            {
                worker.RunDontWait();
            }
            WaitForBackRun();
        }
        [TestMethod]
        public void CuntomDataTest()
        {
            StartHostAsync(m =>
            {
                var data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<CuntomDataBackRun>()
                    .SetData(data)
                    .BuildOnceWorker();
            });
            IOnceWorker worker = GetOnceWorkerByName(nameof(CuntomDataBackRun)).First();
            for (int i = 0; i < 10; i++)
            {
                //await worker.Run();
                worker.RunDontWait();
            }
            Assert.AreEqual("0", worker.GetData("nb"));
            WaitForBackRun();
            string nb = worker.GetData("nb");
            Assert.AreEqual("10", nb);
            Assert.AreEqual(0, worker.Context.exceptNb);
            Assert.AreEqual(10, worker.Context.endNb);
        }
        [TestMethod]
        public async Task CuntomDataTestAwaitAsync()
        {
            StartHostAsync(m =>
            {
                var data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder.Create<CuntomDataBackRun>()
                    .SetData(data)
                    .BuildOnceWorker();
            });
            IOnceWorker worker = GetOnceWorkerByName(nameof(CuntomDataBackRun)).First();
            for (int i = 0; i < 1; i++)
            {
                await worker.Run();
            }
            //Assert.AreEqual("0", worker.GetData("nb"));
            //Assert.AreNotEqual("1000", worker.GetData("nb"));
            WaitForBackRun();
            string nb = worker.GetData("nb");
            Assert.AreEqual("1", nb);
        }


        //多个Backrun
        [TestMethod]
        public void TestSimpleMultAsync()
        {
            StartHostAsync(m =>
            {
                WorkerBuilder
                .Create<SimpleNumberRun>()//内部没有await
                .Add<SimpNbDelayBefore>()
                .Add<SimpNbDelayAfter>()
                .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpleNumberRun)).First();
            work.RunDontWait();
            work.RunDontWait<SimpNbDelayBefore>();
            work.RunDontWait<SimpNbDelayAfter>();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            Assert.AreNotEqual("300", work.GetData("nb"));
            WaitForBackRun();
            //完成之后
            Assert.AreEqual("300", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunDontWaitMultAsync()
        {
            StartHostAsync(m =>
            {
                var data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                WorkerBuilder
                    .Create<SimpleNumberRun>()//内部没有await
                    .SetData(data)
                    .Add<SimpNbDelayBefore>()
                    .Add<SimpNbDelayAfter>()
                    .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(SimpleNumberRun)).First();
            work.RunDontWait();
            work.Run<SimpNbDelayBefore>();
            work.Run<SimpNbDelayAfter>();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");

            //第一个会在这个后面继续运行
            WaitForBackRun();
            Assert.AreEqual("300", work.GetData("nb"));
            Console.WriteLine("进程结束。。。。。。。。。。。。。。");
        }

    }
}
