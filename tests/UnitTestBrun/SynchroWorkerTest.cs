﻿//using Brun;
//using Brun.Workers;
//using BrunTestHelper.BackRuns;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace UnitTestBrun
//{
//    [TestClass]
//    public class SynchroWorkerTest : BaseHostTest
//    {
//        [TestMethod]
//        public void Test()
//        {
//            StartHost(m =>
//            {
//                m.AddBrunService(options =>
//                {
//                    options.WorkerServer = workerServer =>
//                    {
//                        var w = workerServer.CreateSynchroWorker(new WorkerConfig());
//                            w.AddBrun(typeof(SimpleNumberRun),new Brun.Options.OnceBackRunOption());
//                    };
//                });
//                // WorkerBuilder
//                //.Create<SimpleNumberRun>()//内部没有await
//                //.Build<SynchroWorker>();
//            });
//            SynchroWorker work = (SynchroWorker)GetWorkerByName(nameof(SynchroWorker)).First();
//            work.Run();

//            Console.WriteLine("TestSimpleRun：Run 之后的调用线程");

//            Assert.AreEqual(null, work.GetData("nb"));

//            WaitForBackRun();
//            Console.WriteLine("TestSimpleRun：WaitForBackRun 之后的调用线程");
//            Assert.AreEqual("100", work.GetData("nb"));
//        }
//        [TestMethod]
//        public void TestLog()
//        {
//            StartHost(m =>
//            {
//                m.AddBrunService(options =>
//                {
//                    options.WorkerServer = workerServer =>
//                    {
//                        workerServer.CreateSynchroWorker(new WorkerConfig())
//                            .AddBrun(typeof(LogBackRun),new Brun.Options.OnceBackRunOption());
//                    };
//                });
//                // WorkerBuilder
//                //.Create<LogBackRun>()
//                //.Build<SynchroWorker>();
//            });
//            SynchroWorker work = (SynchroWorker)GetWorkerByName(nameof(SynchroWorker)).First();
//            for (int i = 0; i < 100; i++)
//            {
//                work.Run();
//            }
//            Console.WriteLine("TestSimpleRun：WaitForBackRun 之前的调用线程");
//            WaitForBackRun();
//            Console.WriteLine("TestSimpleRun：WaitForBackRun 之后的调用线程");
//            //Assert.AreEqual("100", work.GetData("nb"));
//        }
//    }
//}
