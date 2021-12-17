using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Exceptions;
using Brun.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 简单的内存Queue 自定义数据类型
    /// </summary>
    public class QueueWorker : AbstractWorker, IQueueWorker
    {
        /// <summary>
        /// QueueWorker
        /// </summary>
        /// <param name="config"></param>
        public QueueWorker(WorkerConfig config) : base(config)
        {
            Init();
        }
        private void Init()
        {
            if (string.IsNullOrEmpty(this._config.Key))
            {
                this._config.Key = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(this._config.Name))
            {
                this._config.Name = nameof(QueueWorker);
            }
        }
        /// <summary>
        /// 启动监听线程
        /// </summary>
        /// <returns></returns>
        public override void Start()
        {
            if (_context.State == WorkerState.Started)
            {
                _logger.LogWarning("the QueueWorker key:'{0}' is already started.", _context.Key);
                return;
            }
            if (_context.State != WorkerState.Started)
            {
                _context.State = WorkerState.Started;
                //TODO 减少不必要的多线程开销
                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.IsCancellationRequested && _context.State == WorkerState.Started)
                    {
                        foreach (var item in _backRuns)
                        {
                            QueueBackRun backRun = (QueueBackRun)item.Value;
                            if (backRun.Queue.TryDequeue(out string msg))
                            {
                                var context = new BrunContext(item.Value);
                                context.Message = msg;
                                _ = Execute(context);
                            }
                        }
                        Thread.Sleep(5);
                    }
                }, creationOptions: TaskCreationOptions.LongRunning);
                _logger.LogInformation("the {0} key:'{1}' is started.", GetType().Name, _context.Key);
            }

        }
        /// <summary>
        /// 默认的QueueBackRun,添加消息到后台任务
        /// </summary>
        /// <param name="message"></param>
        public void Enqueue(string message)
        {
            if (this._backRuns.Count > 0)
            {
                ((QueueBackRun)this._backRuns.First().Value).Queue.Enqueue(message);
            }
            else
            {
                _logger.LogError($"the QueueWorker key:'{0}' has no QueuBackRun", this.Key);
            }
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
        /// 指定QueueBackRun类型的消息后台任务，可能多个
        /// </summary>
        /// <param name="queueBackRunType"></param>
        /// <param name="message"></param>
        public void Enqueue(Type queueBackRunType, string message)
        {
            if (message == null)
            {
                _logger.LogWarning($"enqueue message in '{queueBackRunType.Name}' is null, the QueueWorker by key:'{0}' will not execute it.if you want run with empty msg please enqueue ''", this.Key);
                return;
            }
            _backRuns.Where(m => m.Value.GetType() == queueBackRunType).ToList().ForEach(m =>
                {
                    ((QueueBackRun)m.Value).Queue.Enqueue(message);
                });
        }
        /// <summary>
        /// 指定QueueBackRun类型的消息后台任务
        /// </summary>
        /// <param name="queueBackRunTypeFullName">包含命名空间的类型名称</param>
        /// <param name="message"></param>
        public void Enqueue(string queueBackRunTypeFullName, string message)
        {
            var type = Type.GetType(queueBackRunTypeFullName);
            Enqueue(type, message);
        }

        protected override Task Brun(BrunContext context)
        {
            QueueBackRun backrun = (QueueBackRun)context.BackRun;
            return backrun.Run(context.Message, tokenSource.Token);
        }
        public QueueWorker AddBrun(Type queueBackRunType, QueueBackRunOption option)
        {
            if (queueBackRunType == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "queueBackRunType can not be null.");
            if (option == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "QueueBackRunOption can not be null.");
            if (!queueBackRunType.IsSubclassOf(typeof(QueueBackRun)))
            {
                throw new BrunException(BrunErrorCode.TypeError, $"{queueBackRunType.FullName} can not add to QueueWorker.");
            }
            if (_backRuns.Any(m => m.Key == option.Id))
            {
                _logger.LogError("the QueueWorker key:'{0}' has allready added QueueBackRun by id:'{1}' with type:'{2}'.", this.Key, option.Id, queueBackRunType.FullName);
                return this;
            }
            else
            {
                if (option.Id == null)
                    option.Id = Guid.NewGuid().ToString();
                if (option.Name == null)
                    option.Name = queueBackRunType.Name;
                QueueBackRun queueBackRun = (QueueBackRun)BrunTool.CreateInstance(queueBackRunType, option);
                queueBackRun.SetWorkerContext(_context);
                _backRuns.TryAdd(queueBackRun.Id, queueBackRun);
                //InitPreTimeBackRun(queueBackRun);
                _logger.LogInformation("the QueueWorker with key:'{0}' added QueueBackRun by id:'{1}' with type:'{2}' success.", this.Key, option.Id, queueBackRunType.FullName);
                return this;
            }
        }
        public override void Dispose()
        {
            //先停止worker，避免一直监听
            this.Stop();
            base.Dispose();
        }
    }
}
