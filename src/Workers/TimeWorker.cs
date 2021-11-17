using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 简单的时间循环任务，复杂的定时使用<see cref="PlanWorker"/>
    /// //TODO 最简易循环执行任务，继续简化使用
    /// </summary>
    public class TimeWorker : AbstractWorker, ITimeWorker
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public TimeWorker(WorkerConfig config) : base(config)
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
                this._config.Name = nameof(TimeWorker);
            }
        }

        /// <summary>
        /// 启动Worker
        /// </summary>
        /// <returns></returns>
        public override void Start()
        {
            if (_context.State == WorkerState.Started)
            {
                _logger.LogWarning("the TimeWorker key:{0} is already started.", _context.Key);
                return;
            }
            if (_context.State != WorkerState.Started)
            {
                _context.State = WorkerState.Started;
                //TODO 减少不必要的多线程开销
                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested && _context.State == WorkerState.Started)
                    {
                        foreach (var item in _backRuns)
                        {
                            TimeBackRun backRun = (TimeBackRun)item.Value;
                            if (backRun.Option.NextTime != null && backRun.Option.NextTime.Value <= DateTime.Now)
                            {
                                BrunContext brunContext = new BrunContext(backRun);
                                Task.Run(async () =>
                                {
                                    await Execute(brunContext);
                                });
                                backRun.Option.NextTime = DateTime.Now.Add(backRun.Option.Cycle);
                            }
                        }
                        //foreach (var item in TimeOption.CycleTimes)
                        //{
                        //    if (item.NextTime != null && item.NextTime.Value <= DateTime.Now)
                        //    {
                        //        BrunContext brunContext = new BrunContext(item.BackRun.GetType());
                        //        Task.Run(async () =>
                        //        {
                        //            await Execute(brunContext);
                        //        });
                        //        item.NextTime = DateTime.Now.Add(item.Cycle);
                        //    }
                        //}
                        Thread.Sleep(5);
                    }
                }, TaskCreationOptions.LongRunning);
                _logger.LogInformation("the {0} key:{1} is started.", GetType().Name, _context.Key);
            }

            
        }
        protected override Task Brun(BrunContext context)
        {
            return context.BackRun.Run(tokenSource.Token);
        }
        public TimeWorker AddBrun(Type timeBackRunType, TimeBackRunOption option)
        {
            if (!timeBackRunType.IsSubclassOf(typeof(TimeBackRun)))
            {
                throw new BrunTypeErrorException($"{timeBackRunType.FullName} can not add to TimeWorker.");
            }

            if (_backRuns.Any(m => m.Key == option.Id))
            {
                _logger.LogError("the TimeWorker key:'{0}' has allready added timeBackRun by id:'{1}' with type:'{2}'.", this.Key, option.Id, timeBackRunType.FullName);
                return this;
            }
            else
            {
                TimeBackRun timeBackRun = (TimeBackRun)BrunTool.CreateInstance(timeBackRunType, option);
                _backRuns.TryAdd(timeBackRun.Id, timeBackRun);
                InitPreTimeBackRun(timeBackRun);
                _logger.LogInformation("the TimeWorker with key:'{0}' added TimeBackRun by id:'{1}' with type:'{2}' success.", this.Key, option.Id, timeBackRunType.FullName);
                return this;
            }
        }
        /// <summary>
        /// 添加TimeBackRun前的预处理
        /// </summary>
        /// <param name="timeBackRun"></param>
        private void InitPreTimeBackRun(TimeBackRun timeBackRun)
        {
            if (timeBackRun.Option.RunWithStart)
            {
                timeBackRun.Option.NextTime = DateTime.Now.AddSeconds(1);//添加一秒延时
            }
            else
            {
                timeBackRun.Option.NextTime = DateTime.Now.Add(timeBackRun.Option.Cycle);
            }
        }
    }
}
