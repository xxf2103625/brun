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
    //TODO QueueWorker单元测试
    [TestClass]
    public class QueueWorkerTest : BaseHostTest
    {
        [TestMethod]
        public async Task TestExcept()
        {
            string key = nameof(TestExcept);
            WorkerBuilder.CreateQueue<LogQueueBackRun>()
                .SetKey(key)
                .Build();

            await InitAsync().ContinueWith(t =>
            {
                for (int i = 0; i < 100; i++)
                {
                    WorkerServer.Instance.GetQueueWorker(key).Enqueue("测试消息");
                }
            })
                .ContinueWith(async t =>
                {
                    await CleanupAsync();
                });
        }
    }
}
