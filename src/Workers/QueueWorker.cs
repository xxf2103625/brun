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
    /// </summary>
    public class QueueWorker : AbstractWorker, IQueueWorker
    {
        private ConcurrentQueue<string> queue;
        ILogger<QueueWorker> logger;
        private IQueueBackRun _queueBackRun;
        //只需要限制实例并发
        private readonly object backRun_LOCK = new object();
        public QueueWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            queue = new ConcurrentQueue<string>();
        }
        /// <summary>
        /// 实例内保持唯一
        /// </summary>
        protected IQueueBackRun QueueBackRun
        {
            get
            {
                if (_queueBackRun == null)
                {
                    lock (backRun_LOCK)
                    {
                        if (_queueBackRun == null)
                            _queueBackRun = (IQueueBackRun)BrunTool.CreateInstance(_option.BrunType);
                    }
                }
                return _queueBackRun;
            }
        }
        protected async Task Execute(string message)
        {
            //TODO 优化测试代码保证WorkerServer.Instance.ServiceProvider不为null
            logger = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<QueueWorker>>();

            await Observe(WorkerEvents.StartRun);
            try
            {
                await QueueBackRun.Run(message, tokenSource.Token);
            }
            catch (Exception ex)
            {
                _context.ExceptFromRun(ex);
                await Observe(WorkerEvents.Except);
            }
            finally
            {
                await Observe(WorkerEvents.EndRun);
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
                if (!queue.IsEmpty)
                {
                    if (queue.TryDequeue(out string msg))
                    {
                        Task task = Execute(msg);
                        RunningTasks.Add(task);
                        _ = task.ContinueWith(t =>
                          {
                              RunningTasks.TryTake(out t);
                          });
                    }
                }
                await Task.Delay(5);
            }
        }
        /// <summary>
        /// 添加消息到后台任务
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
    }
}
