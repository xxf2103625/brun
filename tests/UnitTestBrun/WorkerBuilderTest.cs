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
    public class WorkerBuilderTest:BaseHostTest
    {
        [TestMethod]
        public void TestCreate()
        {
            string key = Guid.NewGuid().ToString();
            string name = "run_1";
            string tag = "myTag";
            IWorker work = WorkerBuilder.Create<SimpleBackRun>()
                .SetNameTagKey(name, tag, key)
                .Build();
            Assert.AreEqual(key, work.Key);
            Assert.AreEqual(name, work.Name);
            Assert.AreEqual(tag, work.Tag);
        }
        [TestMethod]
        public void TestEmptyCreate()
        {
            IWorker work = WorkerBuilder.Create<SimpleBackRun>()
                .Build();
            Assert.IsNotNull(work.Key);
            Assert.AreEqual(typeof(SimpleBackRun).Name, work.Name);
            Assert.AreEqual("Default", work.Tag);
        }
    }
}
