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
        public QueueWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            queue = new ConcurrentQueue<string>();
        }
        protected async Task Execute(string message)
        {
            ILogger<QueueWorker> logger = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<QueueWorker>>();
            await Observe(WorkerEvents.StartRun);
            IQueueBackRun backRun = (IQueueBackRun)BrunTool.CreateInstance(_option.BrunType);
            await WorkerServer.Instance.TaskFactory.StartNew(async () => await backRun.Run(message, WorkerServer.Instance.StoppingToken))
                .ContinueWith(async task =>
                {
                    switch (task.Status)
                    {
                        case TaskStatus.RanToCompletion:
                            //任务完成，任务可能内部异常
                            logger.LogDebug("TaskStatus.RanToCompletion");
                            logger.LogDebug("this task.Result.Status:{0}", task.Result.Status);
                            switch (task.Result.Status)
                            {
                                case TaskStatus.RanToCompletion:
                                    //任务结果正常完成
                                    logger.LogInformation("task.Result  TaskStatus.RanToCompletion");
                                    break;
                                case TaskStatus.Faulted:
                                    //任务内部异常
                                    logger.LogInformation("task.Result TaskStatus.Faulted");
                                    _context.ExceptFromRun(task.Result.Exception.InnerException);
                                    await Observe(WorkerEvents.Except);
                                    break;
                                case TaskStatus.Canceled:
                                    //任务内部取消
                                    logger.LogInformation("task.Result TaskStatus.Canceled");
                                    _context.ExceptFromRun(new TaskCanceledException("the backrun.Result Task is Canceled"));
                                    await Observe(WorkerEvents.Except);
                                    break;
                            }
                            await Observe(WorkerEvents.EndRun);
                            break;
                        case TaskStatus.Canceled:
                            //任务取消
                            logger.LogDebug("TaskStatus.Canceled");
                            _context.ExceptFromRun(new TaskCanceledException("the backrun Task is Canceled"));
                            await Observe(WorkerEvents.Except);
                            await Observe(WorkerEvents.EndRun);
                            return;
                        case TaskStatus.Faulted:
                            //其它错误
                            logger.LogDebug("TaskStatus.Faulted");
                            _context.ExceptFromRun(new Exception("the backrun Task.TaskStatus is Faulted"));
                            await Observe(WorkerEvents.Except);
                            await Observe(WorkerEvents.EndRun);
                            return;
                    }

                });//, TaskContinuationOptions.ExecuteSynchronously);
        }
        public async Task Start()
        {
            while (!WorkerServer.Instance.StoppingToken.IsCancellationRequested)
            {
                if (queue.TryDequeue(out string msg))
                {
                    Execute(msg);
                }
                await Task.Delay(5);
            }
        }
        public void Start(object token)
        {
            while (!((CancellationToken)token).IsCancellationRequested)
            {
                if(queue.TryDequeue(out string msg))
                {
                    Execute(msg).Start();
                }
            }
        }
        public Task Enqueue(string message)
        {
            //logger = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<QueueWorker>>();
            if (message == null)
            {
                //logger.LogWarning("传入的消息体为null，已忽略");
                return Task.CompletedTask;
            }

            //var queueService = _context.ServiceProvider.GetRequiredService<IBrunQueueService<TMessage>>();
            queue.Enqueue(message);


            return Task.CompletedTask;
        }
    }
}
