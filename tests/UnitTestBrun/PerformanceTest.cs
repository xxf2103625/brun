using Brun;
using Brun.Commons;
using Brun.Options;
using Brun.Workers;
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
    public class PerformanceTest
    {
        private int times = 100000;
        [TestMethod]
        public void TestCreaeObject()
        {

            Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //for (int i = 0; i < times; i++)
            //{
            //    Type type = typeof(SimpleBackRun);
            //     IBackRun backRun= (IBackRun)type.Assembly.CreateInstance(type.FullName);
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"Assembly.CreateInstance,times:{times},elapsed:{stopwatch.ElapsedMilliseconds}");

            //stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < times; i++)
            {
                Type type = typeof(SimpleBackRun);
                BackRun backRun = (BackRun)Activator.CreateInstance(type);
            }
            stopwatch.Stop();
            Console.WriteLine($"Activator.CreateInstance,times:{times},elapsed:{stopwatch.ElapsedMilliseconds}");
        }
        [TestMethod]
        public void TestCreaeObjectWithAgs()
        {
            WorkerConfig config = new WorkerConfig();
            WorkerOption option = new WorkerOption();
            Stopwatch stopwatch = new Stopwatch();


            stopwatch.Start();
            for (int i = 0; i < times; i++)
            {
                Type type = typeof(OnceWorker);
                IWorker worker = (IWorker)BrunTool.CreateInstance(type, args: new object[] { option, config });
                IWorker worker2 = (IWorker)BrunTool.CreateInstance(type, args: new object[] { WorkerServer.Instance.ServerConfig.DefaultOption, WorkerServer.Instance.ServerConfig.DefaultConfig});
            }
            stopwatch.Stop();
            Console.WriteLine($"Activator.CreateInstance,times:{times},elapsed:{stopwatch.ElapsedMilliseconds}");


            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < times; i++)
            {
                Type type = typeof(OnceWorker);
                IWorker worker = (IWorker)type.Assembly.CreateInstance(type.FullName, false, System.Reflection.BindingFlags.Default, null, args: new object[] { option, config }, culture: null, activationAttributes: null);
            }
            stopwatch.Stop();
            Console.WriteLine($"Assembly.CreateInstance,times:{times},elapsed:{stopwatch.ElapsedMilliseconds}");

        }
    }
}
