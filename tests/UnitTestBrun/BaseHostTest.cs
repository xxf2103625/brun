using Brun;
using BrunTestHelper.BackRuns;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    //TODO 重构方便测试子线程的流程
    /// <summary>
    /// 测试Host托管的后台任务基类
    /// </summary>

    public abstract class BaseHostTest
    {
        protected IHost host;
        CancellationToken cancellationToken;
        CancellationTokenSource tokenSource;
        private object LOCK = new object();
        [TestInitialize]
        public void InitAsync()
        {
            WorkerServer.ClearInstance();
        }
        protected void StartHost(Action<IServiceCollection> configure)
        {
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    configure(services);
                    services.AddBrunService();
                })
                .Build();
            host.Start();
        }
        protected IWorker GetWorkerByKey(string key)
        {
            return WorkerServer.Instance.GetWorker(key);
        }
        public IEnumerable<IWorker> GetWorkerByName(string name)
        {
            return WorkerServer.Instance.GetWokerByName(name);
        }
        public IEnumerable<IOnceWorker> GetOnceWorkerByName(string name)
        {
            return WorkerServer.Instance.GetWokerByName(name).Cast<IOnceWorker>();
        }
        public IEnumerable<IWorker> GetWorkerByTag(string tag)
        {
            return WorkerServer.Instance.GetWokerByTag(tag);
        }
        /// <summary>
        /// 等待所有任务完成
        /// </summary>
        /// <param name="runCount">等待任务数</param>
        protected void WaitForBackRun(int runCount = 0)
        {
            WorkerServer server = WorkerServer.Instance;
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            while (server.GetAllWorker().Any(m => m.Context.endNb < m.Context.startNb) || server.GetAllWorker().Any(m => m.Context.endNb < runCount))
            {
                Thread.Sleep(5);
            }
        }
        [TestCleanup]
        public async Task Cleanup()
        {
            Console.WriteLine("---------------------------------------TestCleanup-----------------------------");
            Console.WriteLine("host is null? {0}", host == null);
            if (host != null)
            {
                Task task = host.StopAsync(cancellationToken);
                await task.ContinueWith(t =>
                {
                    Console.WriteLine("stop");
                });
            }
        }
    }
}
