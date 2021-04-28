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
    /// </summary>
    public class QueueWorker : AbstractWorker, IQueueWorker
    {
        public override IEnumerable<Type> BrunTypes => _queueBackRuns.Keys;

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
        /// 启动监听线程
        /// </summary>
        /// <returns></returns>
        public override void Start()
        {
            if (_context.State != WorkerState.Started)
            {
                _context.State = WorkerState.Started;
                //TODO 减少不必要的多线程开销
                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.IsCancellationRequested && _context.State == WorkerState.Started)
                    {
                        foreach (var item in queues)
                        {
                            if (item.Value.TryDequeue(out string msg))
                            {
                                var context = new BrunContext(item.Key);
                                context.Message = msg;
                                Task.Run(async () =>
                                {
                                    await Execute(context);
                                });
                            }
                        }
                        Thread.Sleep(5);
                    }
                }, creationOptions: TaskCreationOptions.LongRunning);
                Logger.LogInformation("the {0} key:{1} is started", GetType().Name, _context.Key);
            }
            Logger.LogWarning("the QueueWorker key:{0} is already started.", _context.Key);
        }
        /// <summary>
        /// 获取BackRun
        /// </summary>
        /// <param name="brunType"></param>
        /// <returns></returns>
        private IQueueBackRun GetQueueBackRun(Type brunType)
        {
            if (_queueBackRuns.TryGetValue(brunType, out IQueueBackRun queueBackRun))
            {
                return queueBackRun;
            }
            else
            {
                Logger.LogDebug("创建新的IQueueBackRun，type:{0}", brunType.Name);
                IQueueBackRun bRun = (IQueueBackRun)BrunTool.CreateInstance(brunType);
                queues.TryAdd(typeof(IQueueBackRun), new ConcurrentQueue<string>());
                _queueBackRuns.TryAdd(brunType, bRun);
                return bRun;
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
                Logger?.LogWarning("传入的消息体为null，已忽略");
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
            IQueueBackRun queueBackRun = GetQueueBackRun(context.BrunType);
            return queueBackRun.Run(context.Message, tokenSource.Token);
        }
        public override void Dispose()
        {
            //先停止worker，避免一直监听
            this.Stop();
            base.Dispose();
        }
    }
}
