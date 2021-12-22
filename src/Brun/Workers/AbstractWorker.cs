using Brun.BaskRuns;
using Brun.Contexts;
using Brun.Enums;
using Brun.Observers;
using Brun.Options;
using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 工作中心基类，可以创建多个，不同实例里面的Context不同
    /// </summary>
    public abstract class AbstractWorker : IWorker
    {
        /// <summary>
        /// 包含的Backrun
        /// </summary>
        protected ConcurrentDictionary<string, IBackRun> _backRuns;
        /// <summary>
        /// 配置
        /// </summary>
        protected WorkerConfig _config;
        /// <summary>
        /// worker上下文
        /// </summary>
        protected WorkerContext _context;
        /// <summary>
        /// 管理单个实例的token
        /// </summary>
        protected CancellationTokenSource tokenSource;
        /// <summary>
        /// 统一配置实例内的Task
        /// </summary>
        protected TaskFactory taskFactory;
        /// <summary>
        /// 状态锁
        /// </summary>
        protected object State_LOCK = new object();
        protected ILogger _logger;
        /// <summary>
        /// 统一构造函数
        /// </summary>
        /// <param name="config"></param>
        public AbstractWorker(WorkerConfig config)
        {
            if (WorkerServer.Instance.ServiceProvider != null)
                _logger = WorkerServer.Instance.LoggerFactory.CreateLogger(this.GetType());
            _config = config;
            _context = new WorkerContext(config);
            _backRuns = new ConcurrentDictionary<string, IBackRun>();
            tokenSource = new CancellationTokenSource();
            taskFactory = new TaskFactory(tokenSource.Token);
            //TODO 控制同时运行并发量
            RunningTasks = new BlockingCollection<Task>();
            _context.RunningTasks = RunningTasks;
        }
        /// <summary>
        /// 内存对象start，对外隐藏
        /// </summary>
        internal abstract void ProtectStart();
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            //转移到注册的服务来处理，方便持久化扩展
            using (var scope = ServiceProvider.CreateScope())
            {
                IWorkerService workerService = scope.ServiceProvider.GetRequiredService<IWorkerService>();
                workerService.Start(this.Key);
            }
        }
        /// <summary>
        /// 统一流程控制
        /// </summary>
        /// <param name="runContext"></param>
        /// <returns></returns>
        protected async Task Execute(BrunContext runContext)
        {
            if (_context.State != WorkerState.Started)
            {
                _logger.LogWarning("the {3} key:'{0}' is not started while {1} is {2} time run", this.Key, runContext.BackRun.GetType(), runContext.StartNb, this.GetType().Name);
                return;
            }
            await Observe(runContext, WorkerEvents.StartRun);
            try
            {
                Task brun = Brun(runContext);
                RunningTasks.TryAdd(brun);
                _ = brun.ContinueWith(t =>
                 {
                     RunningTasks.TryTake(out t);
                 });
                await brun;
            }
            catch (Exception ex)
            {
                _context.ExceptFromRun(ex);
                await Observe(runContext, WorkerEvents.Except);
            }
            finally
            {
                await Observe(runContext, WorkerEvents.EndRun);
            }
        }
        /// <summary>
        /// 执行BackRun.Run
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task Brun(BrunContext context);
        /// <summary>
        /// 内存对象Stop，对外隐藏
        /// </summary>
        internal virtual void ProtectStop()
        {
            _context.State = WorkerState.Stoped;
            _logger.LogInformation("the {0} key:{1} is stoped", GetType().Name, _context.Key);
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var workerService = scope.ServiceProvider.GetRequiredService<IWorkerService>();
                workerService.Stop(this.Key);
            }
        }
        /// <summary>
        /// 添加拦截器
        /// </summary>
        /// <param name="brunContext"></param>
        /// <param name="workerEvents"></param>
        /// <returns></returns>
        protected async Task Observe(BrunContext brunContext, WorkerEvents workerEvents)
        {
            foreach (WorkerObserver observer in _config.GetObservers(workerEvents).OrderBy(m => m.Order))
            {
                await observer.Todo(_context, brunContext);
            }
        }
        public string GetData(string key)
        {
            if (_context.Items == null)
            {
                _logger.LogError("the {0} by key:'{1}' has not config Data.", this.GetType(), key);
                return null;
            }
            if (_context.Items.ContainsKey(key))
                return _context.Items[key];
            else
                return null;
        }
        /// <summary>
        /// 工作中心的Key/Id
        /// </summary>
        public string Key => _config.Key;
        /// <summary>
        /// 工作中心名称
        /// </summary>
        public string Name => _config.Name;
        /// <summary>
        /// 上下文
        /// </summary>
        public WorkerContext Context => _context;
        /// <summary>
        /// 状态
        /// </summary>
        public WorkerState State => _context.State;
        /// <summary>
        /// 包含的Backrun
        /// </summary>
        public ConcurrentDictionary<string, IBackRun> BackRuns => _backRuns;
        /// <summary>
        /// 正在运行的任务
        /// </summary>
        public BlockingCollection<Task> RunningTasks { get; private set; }
        /// <summary>
        /// Ioc容器
        /// </summary>
        protected IServiceProvider ServiceProvider => WorkerServer.Instance.ServiceProvider;
        /// <summary>
        /// 释放单个Worker
        /// </summary>
        public virtual void Dispose()
        {
            DateTime now = DateTime.Now;
            while (_context.endNb < _context.startNb && DateTime.Now - now < _config.TimeWaitForBrun)
            {
                now = now.AddSeconds(0.1);
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
            };
            tokenSource.Cancel();
            this.Context.Dispose();
            //WorkerServer.Instance.Worders.Remove(this);
        }
    }
}
