using Brun.Commons;
using Brun.Enums;
using Brun.Observers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
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
        protected WorkerContext _context;
        protected Task runTask;
        //protected CancellationToken stoppingToken;
        //TODO 管理Task
        //protected TaskFactory taskFactory;
        public AbstractWorker(WorkerOption option, WorkerConfig config)
        {
            _option = option;
            _config = config;
            _context = new WorkerContext(option, config);
            //stoppingToken = WorkerServer.Instance.StoppingToken;
        }

        public virtual Task Destroy()
        {
            throw new NotImplementedException();
        }
        //不能停止运行中的任务，仅停止循环、定时任务触发机制
        public virtual Task Pause()
        {

            throw new NotImplementedException();
        }
        //恢复循环、定时任务触发机制
        public virtual Task Resume()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 调用线程不用等待结果
        /// </summary>
        public void RunDontWait()
        {
            runTask = Run();
        }
        //protected abstract Task Execute();
        public abstract Task Run();

        protected async Task Observe(WorkerEvents workerEvents)
        {
            foreach (var item in _config.GetObservers(workerEvents).OrderBy(m => m.Order))
            {
                await item.Todo(_context);
            }
        }

        public WorkerContext Context => _context;
        public string Key => _option.Key;

        public string Name => _option.Name;

        public string Tag => _option.Tag;
        public Type WorkerType => _option.BrunType;
        public ConcurrentDictionary<string, string> GetData()
        {
            return _context.Items;
        }
        public string GetData(string key)
        {
            if (_context.Items.ContainsKey(key))
                return _context.Items[key];
            else
                return null;
        }
        public T GetData<T>(string key)
        {
            throw new NotImplementedException();
            //var r = GetData(key);
            //if (r == null)
            //    return default;
            //return (T)r;
        }
        public void Dispose()
        {
            DateTime time = DateTime.Now;
            if (runTask == null)
                return;
            //TODO runTask可能会资源竞争
            while (!runTask.IsCompleted)
            {
                //等待BackRun任务结束
                Thread.Sleep(5);
                if ((DateTime.Now - time) > WorkerServer.Instance.ServerConfig.WaitDisposeOutTime)
                {
                    _context.ExceptFromRun(new TimeoutException($"进程结束，BackRun超时{WorkerServer.Instance.ServerConfig.WaitDisposeOutTime.TotalSeconds}秒，已强制取消"));
                    //TODO 优化Worker资源回收
                    IEnumerable<WorkerObserver> exceptRunObservers = _config.GetObservers(WorkerEvents.Except);
                    foreach (var item in exceptRunObservers.OrderBy(m => m.Order))
                    {
                        //默认每个Observer最多等待3秒
                        item.Todo(_context).Wait(TimeSpan.FromSeconds(3));
                    }
                    //TODO 没有执行 WorkerEvents.EndRun
                    break;
                }
            }
            this.Context.Dispose();
        }



        public Task RunningTask => runTask;
    }
}
