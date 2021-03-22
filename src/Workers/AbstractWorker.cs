using Brun.Commons;
using Brun.Enums;
using Brun.Observers;
using Brun.Options;
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
        //TODO 运行中销毁整个实例
        public virtual Task Destroy()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加拦截器
        /// </summary>
        /// <param name="brunType"></param>
        /// <param name="workerEvents"></param>
        /// <returns></returns>
        protected async Task Observe(Type brunType, WorkerEvents workerEvents)
        {
            foreach (var observer in _config.GetObservers(workerEvents).OrderBy(m => m.Order))
            {
                await observer.Todo(_context, brunType);
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
        /// 回收单个Worker
        /// </summary>
        public void Dispose()
        {
            //TODO 控制进程等待时间，加入可配置
            if (RunningTasks.Any(m => m.Status == TaskStatus.WaitingForActivation || m.Status == TaskStatus.Running))
            {
                tokenSource.CancelAfter(TimeSpan.FromSeconds(2));
                tokenSource.Token.Register(() =>
                {
                    Context.Dispose();
                });
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    Thread.Sleep(50);
                }
            }
            else
            {
                tokenSource.Cancel();
                this.Context.Dispose();
            }
        }

        /// <summary>
        /// 正在运行的任务
        /// </summary>
        public BlockingCollection<Task> RunningTasks { get; private set; }

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

        //TODO task管理
        public TaskFactory TaskFactory => taskFactory;
    }
}
