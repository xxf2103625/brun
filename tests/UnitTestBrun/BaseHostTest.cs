using Brun;
using Brun.Services;
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
        IWorkerService workerService;
        IServiceScope scope;
        private object LOCK = new object();
        [TestInitialize]
        public void InitAsync()
        {
            tokenSource = new CancellationTokenSource();
            cancellationToken = tokenSource.Token;
        }
        protected void StartHost(Action<IServiceCollection> configure)
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    configure(services);
                    //services.AddBrunService();
                })
                .Build();
            host.Start();
            scope = host.Services.CreateScope();
            workerService = scope.ServiceProvider.GetRequiredService<IWorkerService>();
        }
        protected IWorker GetWorkerByKey(string key)
        {
            //var workerService = host.Services.GetRequiredService<IWorkerService>();
            return workerService.GetWorkerByKey(key);
            //return WorkerServer.Instance.GetWorker(key);
        }
        public IEnumerable<IWorker> GetWorkerByName(string name)
        {
            //var workerService = host.Services.GetRequiredService<IWorkerService>();
            return workerService.GetWorkerByName(name);
            //return WorkerServer.Instance.GetWokerByName(name);
        }
        public IEnumerable<IOnceWorker> GetOnceWorkerByName(string name)
        {
            //var workerService = host.Services.GetRequiredService<IWorkerService>();
            return workerService.GetWorkerByName(name).Cast<IOnceWorker>();
            //return WorkerServer.Instance.GetWokerByName(name).Cast<IOnceWorker>();
        }

        /// <summary>
        /// //TODO 等待所有任务完成
        /// </summary>
        /// <param name="runCount">等待任务数</param>
        protected void WaitForBackRun(int runCount = 0)
        {
            Console.WriteLine("WaitForBackRun 开始");
            WorkerServer server = WorkerServer.Instance;
            //Thread.Sleep(TimeSpan.FromSeconds(0.1));
            // while (server.Worders.Values.Any(m => m.Context.endNb < m.Context.startNb) || (server.Worders.Values.First().Context.RunningTasks.Count != 0))
            // {
            //     Thread.Sleep(50);
            // }
            
            //Console.WriteLine(server.GetAllWorker().FirstOrDefault());
            while (WorkerServer.BrunIsStart==false)
            {
                Thread.Sleep(50);
            }
            while (server.Worders.Values.Any(m => m.Context.endNb < m.Context.startNb))
            {
                Thread.Sleep(50);
            }
            if (runCount > 0)
            {
                while (server.Worders.Values.Sum(m=>m.Context.endNb)<runCount)
                {
                    //保证所有任务已完成  等待runCount个任务完成
                    Thread.Sleep(50);
                }
            }
            Console.WriteLine("WaitForBackRun 结束");
        }
        protected void WiatAfter(TimeSpan timeSpan)
        {
            DateTime next = DateTime.Now.Add(timeSpan);
            while (DateTime.Now <= next)
            {
                Thread.Sleep(50);
            }
        }
        [TestCleanup]
        public async Task Cleanup()
        {
            Console.WriteLine("---------------------------------------TestCleanup-----------------------------");
            Console.WriteLine("host is null? {0}", host == null);
            if (host != null)
            {
                scope?.Dispose();
                Task task = host.StopAsync();
                await task.ContinueWith(t =>
                {
                    Console.WriteLine("test stop!");
                });
                await task;
            }
        }
    }
}
