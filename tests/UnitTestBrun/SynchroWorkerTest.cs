using Brun;
using Brun.Workers;
using BrunTestHelper.BackRuns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    [TestClass]
    public class SynchroWorkerTest:BaseHostTest
    {
        [TestMethod]
        public void Test()
        {
            StartHost(m =>
            {
                WorkerBuilder
               .Create<SimpleNumberRun>()//内部没有await
               .Build<SynchroWorker>();
            });
            SynchroWorker work = (SynchroWorker)GetWorkerByName(nameof(SimpleNumberRun)).First();
            work.Run();

            Console.WriteLine("TestSimpleRun：Run 之后的调用线程");
            
            Assert.AreEqual(null, work.GetData("nb"));
            
            WaitForBackRun();
            Console.WriteLine("TestSimpleRun：WaitForBackRun 之后的调用线程");
            Assert.AreEqual("100", work.GetData("nb"));
        }
    }
}
