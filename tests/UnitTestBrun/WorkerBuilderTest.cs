﻿using Brun;
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
    public class WorkerBuilderTest : BaseHostTest
    {
        [TestMethod]
        public void TestCreateAsync()
        {
            string key = Guid.NewGuid().ToString();
            string name = "run_1";
            string tag = "myTag";
            StartHostAsync(m =>
            {
                WorkerBuilder.Create<SimpleBackRun>()
               .SetNameTagKey(name, tag, key)
               .Build();
            });
            IWorker work = WorkerServer.Instance.GetWorker(key);
            Assert.AreEqual(key, work.Key);
            Assert.AreEqual(name, work.Name);
            Assert.AreEqual(tag, work.Tag);
        }
        [TestMethod]
        public void TestEmptyCreateAsync()
        {
            StartHostAsync(m =>
           {
               WorkerBuilder.Create<SimpleBackRun>()
               .Build();
           });
            IWorker work = GetWorkerByName(typeof(SimpleBackRun).Name).First();
            Assert.IsNotNull(work.Key);
            Assert.AreEqual(typeof(SimpleBackRun).Name, work.Name);
            Assert.AreEqual("Default", work.Tag);
        }
    }
}
