using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Observers;
using Brun.Options;
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
    /// 简单的内存Queue 自定义数据类型
    ///  TODO 让一个QueueWorker可以配置多个不同类型的QueueBackrun
    /// </summary>
    public class QueueWorker : AbstractWorker, IQueueWorker
    {
        ILogger<QueueWorker> logger;
        private ConcurrentQueue<string> queue => queues[_option.DefaultBrunType];
        private ConcurrentDictionary<Type, ConcurrentQueue<string>> queues;
        private ConcurrentDictionary<Type, IQueueBackRun> _queueBackRuns;
        /// <summary>
        /// QueueWorker
        /// </summary>
        /// <param name="option"></param>
        /// <param name="config"></param>
        public QueueWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            queues = new ConcurrentDictionary<Type, ConcurrentQueue<string>>();
            _queueBackRuns = new ConcurrentDictionary<Type, IQueueBackRun>();
            Init();
        }
        private void Init()
        {
            for (int i = 0; i < _option.BrunTypes.Count; i++)
            {
                queues.TryAdd(_option.BrunTypes[i], new ConcurrentQueue<string>());
                _queueBackRuns.TryAdd(_option.BrunTypes[i], (IQueueBackRun)BrunTool.CreateInstance(_option.BrunTypes[i]));
            }
        }
        /// <summary>
        /// 获取BackRun
        /// </summary>
        /// <param name="brunType"></param>
        /// <returns></returns>
        private IQueueBackRun GetQueueBackRun(Type brunType)
        {
            logger = _context.ServiceProvider?.GetService<ILogger<QueueWorker>>();
            if (_queueBackRuns.TryGetValue(brunType, out IQueueBackRun queueBackRun))
            {
                return queueBackRun;
            }
            else
            {
                logger.LogDebug("创建新的IQueueBackRun，type:{0}", brunType.Name);
                IQueueBackRun bRun = (IQueueBackRun)BrunTool.CreateInstance(brunType);
                _queueBackRuns.TryAdd(brunType, bRun);
                return bRun;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brunType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task Execute(Type brunType, string message)
        {

            Task start = Observe(brunType, WorkerEvents.StartRun);
            await start.ContinueWith(async t =>
             {
                 try
                 {
                     await GetQueueBackRun(brunType).Run(message, tokenSource.Token);
                 }
                 catch (Exception ex)
                 {
                     _context.ExceptFromRun(ex);
                     await Observe(brunType, WorkerEvents.Except);
                 }
                 finally
                 {
                     await Observe(brunType, WorkerEvents.EndRun);
                 }
             });
        }
        private void Run(Type brunType, string message)
        {
            Task task = taskFactory.StartNew(async () =>
           {
               await Execute(brunType, message);
           });

            RunningTasks.Add(task);

            _ = task.ContinueWith(t =>
            {
                RunningTasks.TryTake(out t);

            });
        }
        /// <summary>
        /// 启动QueueWorker
        /// </summary>
        /// <returns></returns>
        public override Task Start()
        {
            if (_context.State == WorkerState.Started)
            {
                return Task.CompletedTask;
            }
            Task start = Task.Factory.StartNew(() =>
             {
                 if (_context.State == WorkerState.Started)
                     return;
                 if (tokenSource != null)
                     tokenSource.Dispose();
                 tokenSource = new CancellationTokenSource();
                 this._context.State = WorkerState.Started;
                 QueueListenning();
                 this._context.State = WorkerState.Stoped;
             }, creationOptions: TaskCreationOptions.LongRunning);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 停止QueueWorker
        /// </summary>
        /// <returns></returns>
        public override Task Stop()
        {
            if (_context.State != WorkerState.Started)
                return Task.CompletedTask;
            DateTime now = DateTime.Now;
            while (_context.endNb < _context.startNb && DateTime.Now - now < _config.TimeWaitForBrun)
            {
                now = now.AddSeconds(0.1);
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
            };
            tokenSource.Cancel();
            return Task.CompletedTask;
        }
        private void QueueListenning()
        {
            while (!tokenSource.IsCancellationRequested)
            {
                foreach (var item in queues)
                {
                    if (item.Value.TryDequeue(out string msg))
                    {
                        Run(item.Key, msg);
                    }
                }
            }
        }
        /// <summary>
        /// 默认的QueueBackRun,添加消息到后台任务
        /// </summary>
        /// <param name="message"></param>
        public void Enqueue(string message)
        {
            Enqueue(_option.DefaultBrunType, message);
        }
        /// <summary>
        /// 指定QueueBackRun类型的消息后台任务
        /// </summary>
        /// <typeparam name="TQueueBackRun"></typeparam>
        /// <param name="message"></param>
        public void Enqueue<TQueueBackRun>(string message)
        {
            Enqueue(typeof(TQueueBackRun), message);
        }
        /// <summary>
        /// 指定QueueBackRun类型的消息后台任务
        /// </summary>
        /// <param name="queueBackRunType"></param>
        /// <param name="message"></param>
        public void Enqueue(Type queueBackRunType, string message)
        {
            if (message == null)
            {
                logger?.LogWarning("传入的消息体为null，已忽略");
            }
            queues[queueBackRunType].Enqueue(message);
        }
        /// <summary>
        /// 指定QueueBackRun类型的消息后台任务
        /// </summary>
        /// <param name="queueBackRunTypeFullName"></param>
        /// <param name="message"></param>
        public void Enqueue(string queueBackRunTypeFullName, string message)
        {
            var type = Type.GetType(queueBackRunTypeFullName);
            Enqueue(type, message);
        }

        protected override Task Brun(BrunContext context)
        {
            throw new NotImplementedException();
        }
    }
}
