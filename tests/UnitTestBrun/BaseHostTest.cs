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
        //TimeSpan waitTime = TimeSpan.FromSeconds(3);
        private object LOCK = new object();
        [TestInitialize]
        public void InitAsync()
        {
            WorkerServer.ClearInstance();
            //tokenSource = new CancellationTokenSource();
            //cancellationToken = tokenSource.Token;
            //host = Host.CreateDefaultBuilder()
            //    .ConfigureServices((hostContext, services) =>
            //    {
            //        services.AddBrunService();
            //    })
            //    .Build();
            //await host.StartAsync(cancellationToken);
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
            //await Task.Delay(TimeSpan.FromSeconds(0.1));//防止任务还没启动就结束
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
        /// <returns></returns>
        protected void WaitForBackRun()
        {
            WorkerServer server = WorkerServer.Instance;
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
            while (server.GetAllWorker().Any(m => m.Context.endNb < m.Context.startNb))
            {
                Thread.Sleep(5);
            }
        }
        [TestCleanup]
        public async Task CleanupAsync()
        {
            if (host != null)
            {
                await host.StopAsync();
                host = null;
            }
                
            ////tokenSource.Cancel();
            //if (host != null)
            //{

            //    //host = null;
            //}
            //return Task.CompletedTask;
        }
    }
}
