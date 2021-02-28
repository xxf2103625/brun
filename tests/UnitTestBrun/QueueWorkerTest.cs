using Brun;
using BrunTestHelper.QueueBackRuns;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    [TestClass]
    public class QueueWorkerTest:BaseHostTest
    {
        [TestMethod]
        public async Task TestExcept()
        {
            string key = nameof(TestExcept);
            WorkerBuilder.CreateQueue<ErrorQueueBackRun>()
                .SetKey(key)
                .Build();

            await InitAsync();

            await WorkerServer.Instance.GetQueueWorker(key).Enqueue("msg");

            await Task.Delay(TimeSpan.FromSeconds(5));
        } 
    }
}
