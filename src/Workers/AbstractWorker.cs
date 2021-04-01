using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Observers;
using Brun.Options;
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
    /// 工作中心基类，可以builder多个，不同实例里面的Context不同
    /// </summary>
    public abstract class AbstractWorker : IWorker
    {
        /// <summary>
        /// 选项
        /// </summary>
        protected WorkerOption _option;
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
        /// <summary>
        /// 统一构造函数
        /// </summary>
        /// <param name="option"></param>
        /// <param name="config"></param>
        public AbstractWorker(WorkerOption option, WorkerConfig config)
        {
            _option = option;
            _config = config;
            _context = new WorkerContext(option, config);
            tokenSource = new CancellationTokenSource();
            taskFactory = new TaskFactory(tokenSource.Token);
            //TODO 控制同时运行并发量
            RunningTasks = new BlockingCollection<Task>();
            _context.Tasks = RunningTasks;
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public virtual Task Start()
        {
            _context.State = WorkerState.Started;
            Logger.LogInformation("the {0} key:{1} is started", GetType().Name, _context.Key);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 统一流程控制
        /// </summary>
        /// <param name="runContext"></param>
        /// <returns></returns>
        protected Task Execute(BrunContext runContext)
        {
            Task before = Observe(runContext, WorkerEvents.StartRun);
            _ = before.ContinueWith(async t =>
              {
                  try
                  {
                      if (_context.State != WorkerState.Started)
                      {
                          Logger.LogWarning("the worker is not started while {0} is {1} time run", runContext.BrunType.Name, runContext.StartNb);
                          return;
                      }
                      Task task = Brun(runContext);
                      RunningTasks.TryAdd(task);
                      _ = task.ContinueWith(tbrun =>
                        {
                            RunningTasks.TryTake(out tbrun);
                        });
                      await task;
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
              }, TaskContinuationOptions.ExecuteSynchronously);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 执行BackRun.Run
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract Task Brun(BrunContext context);
        public abstract IEnumerable<Type> BrunTypes { get; }
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public virtual Task Stop()
        {
            _context.State = WorkerState.Stoped;
            Logger.LogInformation("the {0} key:{1} is stoped", GetType().Name, _context.Key);
            return Task.CompletedTask;
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
        /// <summary>
        /// 上下文
        /// </summary>
        public WorkerContext Context => _context;
        public string Key => _option.Key;

        public string Name => _option.Name;

        public string Tag => _option.Tag;
        public Type WorkerType => _option.WorkerType;

        public string GetData(string key)
        {
            if (_context.Items.ContainsKey(key))
                return _context.Items[key];
            else
                return null;
        }
        /// <summary>
        /// 正在运行的任务
        /// </summary>
        public BlockingCollection<Task> RunningTasks { get; private set; }
        protected IServiceProvider ServiceProvider => WorkerServer.Instance.ServiceProvider;
        protected ILoggerFactory LoggerFactory => (ILoggerFactory)ServiceProvider.GetService(typeof(ILoggerFactory));
        protected ILogger Logger => LoggerFactory.CreateLogger(this.GetType());
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="TWorker"></typeparam>
        /// <returns></returns>
        public TWorker As<TWorker>() where TWorker : AbstractWorker
        {
            return (TWorker)this;
        }
        public IOnceWorker AsOnceWorker()
        {
            return (IOnceWorker)this;
        }
        public IQueueWorker AsQueueWorker()
        {
            return (IQueueWorker)this;
        }
        public ITimeWorker AsTimeWOrker()
        {
            return (ITimeWorker)this;
        }
        public IPlanTimeWorker AsPlanTimeWorker()
        {
            return (IPlanTimeWorker)this;
        }
        public TaskFactory TaskFactory => taskFactory;
        /// <summary>
        /// 释放单个Worker
        /// </summary>
        public void Dispose()
        {
            DateTime now = DateTime.Now;
            while (_context.endNb < _context.startNb && DateTime.Now - now < _config.TimeWaitForBrun)
            {
                now = now.AddSeconds(0.1);
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
            };
            tokenSource.Cancel();
            this.Context.Dispose();
        }
    }
}
