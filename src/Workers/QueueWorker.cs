using Brun.BaskRuns;
using Brun.Commons;
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
        private IQueueBackRun _queueBackRun;
        private ConcurrentDictionary<Type, IQueueBackRun> _queueBackRuns;
        //只需要限制实例并发
        //private readonly object backRun_LOCK = new object();
        public QueueWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            //queue = new ConcurrentQueue<string>();
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
        /// 获取BackRun //TODO 测试多线程安全
        /// </summary>
        /// <param name="brunType"></param>
        /// <returns></returns>
        protected IQueueBackRun GetQueueBackRun(Type brunType)
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
        /// <param name="burnType"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected async Task Execute(Type brunType, string message)
        {

            await Observe(brunType, WorkerEvents.StartRun);
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
        }
        /// <summary>
        /// 启动QueueWorker
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            while (!this.tokenSource.Token.IsCancellationRequested)
            {
                foreach (var item in queues)
                {
                    if (item.Value.TryDequeue(out string msg))
                    {
                        RunningTasks.Add(TaskFactory.StartNew(() =>
                        {
                            Task task = Execute(item.Key, msg);
                            task.ContinueWith(t => RunningTasks.TryTake(out t));

                        }));
                    }
                }
                //if (!queue.IsEmpty)
                //{
                //    if (queue.TryDequeue(out string msg))
                //    {
                //        Task task = Execute(msg);
                //        RunningTasks.Add(task);
                //        _ = task.ContinueWith(t =>
                //          {
                //              RunningTasks.TryTake(out t);
                //          });
                //    }
                //}
                await Task.Delay(5);
            }
        }
        /// <summary>
        /// 默认的QueueBackRun,添加消息到后台任务
        /// </summary>
        /// <param name="message"></param>
        public void Enqueue(string message)
        {
            if (message == null)
            {
                logger?.LogWarning("传入的消息体为null，已忽略");
            }
            queue.Enqueue(message);
        }
        /// <summary>
        /// 指定QueueBackRun类型的消息后台任务
        /// </summary>
        /// <typeparam name="TQueueBackRun"></typeparam>
        /// <param name="message"></param>
        public void Enqueue<TQueueBackRun>(string message)
        {
            if (message == null)
            {
                logger?.LogWarning("传入的消息体为null，已忽略");
            }
            queues[typeof(TQueueBackRun)].Enqueue(message);
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
            if (message == null)
            {
                logger?.LogWarning("传入的消息体为null，已忽略");
            }
            var type = Type.GetType(queueBackRunTypeFullName);
            queues[type].Enqueue(message);
        }
    }
}
