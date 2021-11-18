using Brun;
using Brun.Services;
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
    public class PlanTimeWorkerTest : BaseHostTest
    {
        [TestMethod]
        public void TestPlanTimeSystem()
        {
            //WorkerServer.Instance.ServerConfig.UseSystemBrun = true;
            //StartHost(config =>
            //{
            //    //WorkerBuilder.CreatePlanTime<SystemBackRun>("* * * * * ")
            //    //       .SetKey(SystemBackRun.Worker_KEY)
            //    //       .SetName("Brun系统监控")
            //    //       .Build()
            //    //       ;
            //});
            //Thread.Sleep(TimeSpan.FromSeconds(3));
            //WaitForBackRun();

        }
    }
}
