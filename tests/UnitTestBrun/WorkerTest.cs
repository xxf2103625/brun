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
     *work.Run()，强制线程池执行，并丢弃子线程信息，主线程都不会等待
     *当BackRun中有await时，使用work.Run()不要await 才和Run()效果一样
     */
    [TestClass]
    public class WorkerTest : BaseHostTest
    {
        [TestMethod]
        public void TestSimpleRunAsync()
        {
            StartHost(m =>
            {
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).AddBrun(typeof(SimpleNumberRun), new Brun.Options.OnceBackRunOption());
                    };
                });
                // WorkerBuilder
                //.Create<SimpleNumberRun>()//内部没有await
                //.BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //不会等待
            Assert.AreEqual(null, work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRun()
        {
            StartHost(m =>
            {
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).AddBrun<SimpleNumberRun>();
                    };
                });
                //WorkerBuilder
                //.Create<SimpleNumberRun>()//内部没有await
                //.BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //等待
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));

        }
        [TestMethod]
        public void TestSimpleRunAsync_2()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBefore>();
                    };
                });

                //WorkerBuilder.Create<SimpNbDelayBefore>()//内部有await
                //   .SetData(data)
                //   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //不等待 //backrun内部用了await
            Assert.AreEqual("0", work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBefore>();
                    };
                });
                //WorkerBuilder.Create<SimpNbDelayBefore>()// 内部有await
                //   .SetData(data)
                //   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //等待结果
            WaitForBackRun();
            Console.WriteLine("UI线程结束");
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeAsyncAwait()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBefore>();
                    };
                });
                // WorkerBuilder.Create<SimpNbDelayBefore>()// 内部await
                //.SetData(data)
                //.BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //不会等待结果
            Assert.AreEqual("0", work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskWaitWorkerVoidAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBeforeTask>();
                    };
                });
                //WorkerBuilder.Create<SimpNbDelayBeforeTask>() //内部没有await
                //   .SetData(data)
                //   .BuildOnceWorker();
            });

            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //不会等待，
            Assert.AreEqual("0", work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskWaitWorkerAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBeforeTask>();
                    };
                });
                //WorkerBuilder.Create<SimpNbDelayBeforeTask>() //内部没有await
                //   .SetData(data)
                //   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //不会等待， 
            Assert.AreEqual("0", work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeVoidAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBefore>();
                    };
                });
                //WorkerBuilder.Create<SimpNbDelayBefore>()//run内使用了async
                //   .SetData(data)
                //   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //不会等待任务
            Assert.AreEqual("0", work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestAsyncSimpleRunBefore()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBefore>();
                    };
                });
                //WorkerBuilder.Create<SimpNbDelayBefore>()//run内使用了async
                //    .SetData(data)
                //    .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //不等待任务
            Assert.AreEqual("0", work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskVoidAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()
                        {
                            TimeWaitForBrun = TimeSpan.FromSeconds(5)
                        }).SetData(data).AddBrun<SimpNbDelayBeforeTask>();
                    };
                });
                //WorkerBuilder.Create<SimpNbDelayBeforeTask>() //return Task.CompletedTask;
                //   .SetData(data)
                //   //自定义等待任务结束时间
                //   .SetConfig(config =>
                //   {
                //       config.TimeWaitForBrun = TimeSpan.FromSeconds(5);
                //   })
                //   .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();

            Assert.AreEqual("0", work.GetData("nb"));
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunBeforeTaskAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpNbDelayBeforeTask>();
                    };
                });
                //IOnceWorker work = WorkerBuilder.Create<SimpNbDelayBeforeTask>() //return Task.CompletedTask;
                //    .SetData(data)
                //    .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            //等待任务
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
        [TestMethod]
        public void TestErrorNb()
        {
            int max = 100;
            string key = Guid.NewGuid().ToString();
            StartHost(m =>
            {
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig(key, "n")).AddBrun<ErrorBackRun3>();
                    };
                });
                //WorkerBuilder.Create<ErrorBackRun3>() //await
                //    .SetKey(key)
                //  .Build();
            });
            IWorker work = GetWorkerByKey(key); //GetOnceWorkerByName(nameof(ErrorBackRun3)).First();

            for (int i = 0; i < max; i++)
            {
                ((IOnceWorker)work).Run();
            }
            Console.WriteLine($"before start:{work.Context.startNb},end:{work.Context.endNb},except:{work.Context.exceptNb}");
            //TODO 批量单元测试时 >0
            //Assert.AreEqual(0, work.Context.startNb);
            Assert.AreNotEqual(max, work.Context.startNb);
            Assert.AreEqual(0, work.Context.exceptNb);
            WaitForBackRun(100);
            Console.WriteLine($"after start:{work.Context.startNb},end:{work.Context.endNb},except:{work.Context.exceptNb}");
            Assert.AreEqual(max, work.Context.startNb);
            Assert.AreEqual(max, work.Context.exceptNb);
            Assert.AreEqual(max, work.Context.endNb);
        }
        [TestMethod]
        public void TaskTestErrorRunNotVoidAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<ErrorBackRun3>();
                    };
                });
                //WorkerBuilder.Create<ErrorBackRun3>() //await
                //   .SetData(data)
                //  .BuildOnceWorker();
            });
            OnceWorker work = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            Assert.AreEqual(0, work.Context.startNb);
            Assert.AreEqual(0, work.Context.exceptNb);
            Assert.AreEqual(0, work.Context.endNb);
            Console.WriteLine($"before start:{work.Context.startNb},end:{work.Context.endNb},except:{work.Context.exceptNb}");
            WaitForBackRun();
            Console.WriteLine($"after start:{work.Context.startNb},end:{work.Context.endNb},except:{work.Context.exceptNb}");
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public void TaskTestRunNotAwaitAsync()
        {
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<DataBackRun>();
                    };
                });
                //WorkerBuilder.Create<DataBackRun>()//await
                //    .SetData(data)
                //   .BuildOnceWorker();
            });
            OnceWorker work = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            //Assert.AreEqual(0, work.Context.startNb);
            Assert.AreEqual(0, work.Context.endNb);
            Console.WriteLine($"before start:{work.Context.startNb},end:{work.Context.endNb}");
            //等待
            WaitForBackRun();
            Console.WriteLine($"after start:{work.Context.startNb},end:{work.Context.endNb}");
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
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<DataBackRun>();
                    };
                });
                //WorkerBuilder.Create<DataBackRun>()//await
                //    .SetData(data)
                //   .BuildOnceWorker();
            });
            OnceWorker work = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
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
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig() { TimeWaitForBrun = TimeSpan.FromSeconds(10) }).SetData(data).AddBrun<ErrorLongBackRun>();
                    };
                });
                //WorkerBuilder.Create<ErrorLongBackRun>() //await
                //   .SetData(data)
                //   .SetConfig(m =>
                //   {
                //       m.TimeWaitForBrun = TimeSpan.FromSeconds(10);
                //   })
                //   .BuildOnceWorker();
            });
            OnceWorker work = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
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
            StartHost(m =>
            {
                ConcurrentDictionary<string, string> data = new ConcurrentDictionary<string, string>();
                data["b"] = "2";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig() { TimeWaitForBrun = TimeSpan.FromSeconds(5) }).SetData(data).AddBrun<ErrorBackRun4>();
                    };
                });
                //WorkerBuilder.Create<ErrorBackRun4>()
                //    .SetData(data)
                //    .SetConfig(m =>
                //    {
                //        m.TimeWaitForBrun = TimeSpan.FromSeconds(5);
                //    })
                //   .BuildOnceWorker();
            });
            OnceWorker work = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            Assert.AreEqual(0, work.Context.exceptNb);
            WaitForBackRun();
            //任务等待主线程
            Assert.AreEqual("1", work.GetData("a"));
            Assert.AreEqual("2", work.GetData("b"));
            Assert.AreEqual(10, work.Context.exceptNb);
        }
        [TestMethod]
        public void TaskTestErrorRunAwaitAsync()
        {
            StartHost(m =>
            {
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).AddBrun<ErrorBackRun>();
                    };
                });
                // WorkerBuilder.Create<ErrorBackRun>()
                //.BuildOnceWorker();
            });
            OnceWorker work = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            Assert.AreEqual(0, work.Context.exceptNb);
            WaitForBackRun();
            Assert.AreEqual(1, work.Context.exceptNb);
        }
        public static int SyTest = 0;
        //[TestMethod]
        //public void SynchroWorkerTest()
        //{
        //    SimpleBackRun.SimNb = 0;
        //    int max = 10;
        //    StartHost(m =>
        //    {
        //        m.AddBrunService(options =>
        //        {
        //            options.WorkerServer = workerServer =>
        //            {
        //                workerServer.CreateSynchroWorker(new WorkerConfig()).AddBrun<SimpleBackRun>();
        //            };
        //        });
        //        //WorkerBuilder.Create<SimpleBackRun>()
        //        //    .Build<SynchroWorker>();
        //        //.Build();
        //    });
        //    SynchroWorker worker = (SynchroWorker)GetWorkerByName(nameof(SynchroWorker)).First();
        //    for (int i = 0; i < max; i++)
        //    {
        //        worker.Run();
        //    }
        //    Console.WriteLine("主线程 WaitForBackRun 之前");
        //    Assert.AreEqual(0, SimpleBackRun.SimNb);
        //    WaitForBackRun(max);
        //    Console.WriteLine("主线程 WaitForBackRun 之后");
        //    Assert.AreEqual(max, SimpleBackRun.SimNb);
        //}
        //[TestMethod]
        //public void SynchroWorkerTestLong()
        //{
        //    SimpleLongBackRun.SimNb = 0;
        //    int max = 10;
        //    StartHost(m =>
        //    {
        //        m.AddBrunService(options =>
        //        {
        //            options.WorkerServer = workerServer =>
        //            {
        //                workerServer.CreateSynchroWorker(new WorkerConfig()).AddBrun<SimpleLongBackRun>();
        //            };
        //        });
        //        //WorkerBuilder.Create<SimpleLongBackRun>()
        //        //    .Build<SynchroWorker>();
        //        //.Build();
        //    });
        //    SynchroWorker worker = (SynchroWorker)GetWorkerByName(nameof(SynchroWorker)).First();
        //    for (int i = 0; i < max; i++)
        //    {
        //        worker.Run();
        //    }
        //    Console.WriteLine("主线程 WaitForBackRun 之前");
        //    Assert.AreEqual(0, SimpleLongBackRun.SimNb);
        //    WaitForBackRun(max);
        //    Console.WriteLine("主线程 WaitForBackRun 之后");
        //    Assert.AreEqual(max, SimpleLongBackRun.SimNb);
        //}
        //[TestMethod]
        //public void SynchroWorkerTestVoid()
        //{
        //    SimpleBackRun.SimNb = 0;
        //    int max = 10;
        //    StartHost(m =>
        //    {
        //        m.AddBrunService(options =>
        //        {
        //            options.WorkerServer = workerServer =>
        //            {
        //                workerServer.CreateSynchroWorker(new WorkerConfig()).AddBrun<SimpleBackRun>();
        //            };
        //        });
        //        //WorkerBuilder.Create<SimpleBackRun>()
        //        //    .Build<SynchroWorker>();
        //    });
        //    SynchroWorker worker = (SynchroWorker)GetWorkerByName(nameof(SynchroWorker)).First();
        //    for (int i = 0; i < max; i++)
        //    {
        //        worker.Run();
        //    }
        //    Assert.AreEqual(0, SimpleBackRun.SimNb);
        //    WaitForBackRun(max);
        //    Assert.AreEqual(max, SimpleBackRun.SimNb);
        //}
        //[TestMethod]
        //public void SynchroWorkerManyTest()
        //{
        //    StartHost(m =>
        //    {
        //        m.AddBrunService(options =>
        //        {
        //            options.WorkerServer = workerServer =>
        //            {
        //                workerServer.CreateSynchroWorker(new WorkerConfig()).AddBrun<SimpeManyBackRun>();
        //            };
        //        });
        //        //WorkerBuilder.Create<SimpeManyBackRun>()
        //        //             .Build<SynchroWorker>();
        //    });
        //    SynchroWorker worker = (SynchroWorker)GetWorkerByName(nameof(SynchroWorker)).First();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        worker.Run();
        //    }
        //    WaitForBackRun();
        //    Assert.AreEqual(3, worker.Context.endNb);
        //}
        //[TestMethod]
        //public void SynchroWorkerManyDontWaitTestAsync()
        //{
        //    StartHost(m =>
        //    {
        //        m.AddBrunService(options =>
        //        {
        //            options.WorkerServer = workerServer =>
        //            {
        //                workerServer.CreateSynchroWorker(new WorkerConfig()).AddBrun<SimpeManyBackRun>();
        //            };
        //        });
        //        // WorkerBuilder.Create<SimpeManyBackRun>().SetWorkerType(typeof(SynchroWorker))
        //        //.BuildOnceWorker();
        //    });
        //    SynchroWorker worker = (SynchroWorker)GetWorkerByName(nameof(SynchroWorker)).First();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        worker.Run();
        //    }
        //    Console.WriteLine($"before start:{worker.Context.startNb},end:{worker.Context.endNb}");
        //    Assert.AreEqual(0, worker.Context.startNb);
        //    Assert.AreEqual(0, worker.Context.endNb);
        //    WaitForBackRun(3);
        //    Console.WriteLine($"after start:{worker.Context.startNb},end:{worker.Context.endNb}");
        //    Assert.AreEqual(3, worker.Context.endNb);
        //}
        [TestMethod]
        public void CuntomDataTest()
        {
            StartHost(m =>
            {
                var data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<CuntomDataBackRun>();
                    };
                });
                //WorkerBuilder.Create<CuntomDataBackRun>(data)
                //    //.SetData(data)
                //    .BuildOnceWorker();
            });
            OnceWorker worker = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            for (int i = 0; i < 10; i++)
            {
                //await worker.Run();
                worker.Run();
            }
            Assert.AreEqual("0", worker.GetData("nb"));
            WaitForBackRun(10);
            string nb = worker.GetData("nb");
            Assert.AreEqual("10", nb);
            Assert.AreEqual(0, worker.Context.exceptNb);
            Assert.AreEqual(10, worker.Context.endNb);
        }
        [TestMethod]
        public void CuntomDataTestAwaitAsync()
        {
            StartHost(m =>
            {
                var data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<CuntomDataBackRun>();
                    };
                });
                //WorkerBuilder.Create<CuntomDataBackRun>()
                //    .SetData(data)
                //    .BuildOnceWorker();
            });
            IOnceWorker worker = GetOnceWorkerByName(nameof(OnceWorker)).First();
            
            for (int i = 0; i < 1; i++)
            {
                worker.Run();
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
            StartHost(m =>
            {
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        var worker = workerServer.CreateOnceWorker(new WorkerConfig());
                        worker.AddBrun<SimpleNumberRun>();
                        worker.AddBrun<SimpNbDelayBefore>(); worker.AddBrun<SimpNbDelayAfter>();
                    };
                });
                //WorkerBuilder
                //.Create<SimpleNumberRun>()//内部没有await
                //.Add<SimpNbDelayBefore>()
                //.Add<SimpNbDelayAfter>()
                //.BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            work.Run<SimpNbDelayBefore>();
            work.Run<SimpNbDelayAfter>();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            Assert.AreNotEqual("300", work.GetData("nb"));
            WaitForBackRun();
            //完成之后
            Assert.AreEqual("300", work.GetData("nb"));
        }
        [TestMethod]
        public void TestSimpleRunMultAsync()
        {
            StartHost(m =>
            {
                var data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        var worker = workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data);
                        worker.AddBrun<SimpleNumberRun>();
                        worker.AddBrun<SimpNbDelayBefore>();
                        worker.AddBrun<SimpNbDelayAfter>();
                    };
                });
                //WorkerBuilder
                //    .Create<SimpleNumberRun>()//内部没有await
                //    .SetData(data)
                //    .Add<SimpNbDelayBefore>()
                //    .Add<SimpNbDelayAfter>()
                //    .BuildOnceWorker();
            });
            IOnceWorker work = GetOnceWorkerByName(nameof(OnceWorker)).First();
            work.Run();
            work.Run<SimpNbDelayBefore>();
            work.Run<SimpNbDelayAfter>();
            Console.WriteLine("TestSimpleRun：await Run() 之后的调用线程");
            //任务还没跑完
            Assert.AreNotEqual("300", work.GetData("nb"));

            WaitForBackRun();
            Assert.AreEqual("300", work.GetData("nb"));
            Console.WriteLine("进程结束。。。。。。。。。。。。。。");
        }
        [TestMethod]
        public void TestSimpleRunStop()
        {
            StartHost(m =>
            {
                var data = new ConcurrentDictionary<string, string>();
                data["nb"] = "0";
                m.AddBrunService(options =>
                {
                    options.WorkerServer = workerServer =>
                    {
                        workerServer.CreateOnceWorker(new WorkerConfig()).SetData(data).AddBrun<SimpleNumberRun>();
                    };
                });
                //WorkerBuilder
                //    .Create<SimpleNumberRun>()//内部没有await
                //    .SetData(data)
                //    .BuildOnceWorker();
            });
            OnceWorker work = (OnceWorker)GetOnceWorkerByName(nameof(OnceWorker)).First();
            //work.Run();
            //任务还没跑完
            Assert.AreEqual("0", work.GetData("nb"));


            work.Stop();
            for (int i = 0; i < 10; i++)
            {
                work.Run();
            }
            WaitForBackRun();
            Assert.AreEqual("0", work.GetData("nb"));

            work.Start();
            work.Run();
            WaitForBackRun();
            Assert.AreEqual("100", work.GetData("nb"));
        }
    }
}
