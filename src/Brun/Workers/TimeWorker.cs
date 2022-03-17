using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Exceptions;
using Brun.Models;
using Brun.Options;
using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
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
        internal override void ProtectStart()
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
                Task.Factory.StartNew((Action)(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested && _context.State == WorkerState.Started)
                    {
                        foreach (var item in _backRuns)
                        {
                            TimeBackRun backRun = (TimeBackRun)item.Value;
                            if (backRun.Option.NextTime != null && backRun.Option.NextTime.Value <= DateTime.Now)
                            {
                                base.taskFactory.StartNew(() =>
                                {
                                    BrunContext brunContext = new BrunContext(backRun);
                                    _ = Execute(brunContext);
                                });
                                _logger.LogInformation($"TimeWorker with key '{this.Key}' is executing,backrun name:'{backRun.Name}',id:'{item.Key}'.");
                                backRun.Option.NextTime = DateTime.Now.Add(backRun.Option.Cycle);
                            }
                        }
                        Thread.Sleep(5);
                    }
                }), TaskCreationOptions.LongRunning)
                    .ContinueWith(t => { _context.State = WorkerState.Default; });
                _logger.LogInformation("the {0} key:{1} is started.", GetType().Name, _context.Key);
            }
        }
        protected override Task Brun(BrunContext context)
        {
            return ((TimeBackRun)context.BackRun).Run(tokenSource.Token);
        }
        public ITimeWorker AddBrun(Type timeBackRunType, TimeBackRunOption option)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var timeBrunService = scope.ServiceProvider.GetRequiredService<ITimeBrunService>();
                return timeBrunService.AddTimeBrun(this, timeBackRunType, option);
            }
        }
        public ITimeWorker AddBrun<TTimeBackRun>(TimeBackRunOption option) where TTimeBackRun : TimeBackRun
        {
            return this.AddBrun(typeof(TTimeBackRun), option);
        }
        //对外隐藏
        internal ITimeWorker ProtectAddBrun(Type timeBackRunType, TimeBackRunOption option)
        {
            if (timeBackRunType == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "timeBackRunType can not be null.");
            if (option == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "TimeBackRunOption can not be null.");
            if (!timeBackRunType.IsSubclassOf(typeof(TimeBackRun)))
            {
                throw new BrunException(BrunErrorCode.TypeError, $"{timeBackRunType.FullName} can not add to TimeWorker.");
            }

            if (_backRuns.Any(m => m.Key == option.Id))
            {
                throw new BrunException(BrunErrorCode.AllreadyKey, "the TimeWorker key:'{0}' has allready added timeBackRun by id:'{1}' with type:'{2}'.", this.Key, option.Id, timeBackRunType.FullName);
                //_logger.LogError("the TimeWorker key:'{0}' has allready added timeBackRun by id:'{1}' with type:'{2}'.", this.Key, option.Id, timeBackRunType.FullName);
                //return this;
                //return BrunResultState.IdBeUsed;
            }
            else
            {
                if (option.Id == null)
                    option.Id = Guid.NewGuid().ToString();
                if (option.Name == null)
                    option.Name = timeBackRunType.Name;
                TimeBackRun timeBackRun = (TimeBackRun)BrunTool.CreateInstance(timeBackRunType, option);
                timeBackRun.SetWorkerContext(_context);
                if (_backRuns.TryAdd(timeBackRun.Id, timeBackRun))
                {
                    InitPreTimeBackRun(timeBackRun);
                    _logger.LogInformation("the TimeWorker with key:'{0}' added TimeBackRun by id:'{1}' with type:'{2}' success.", this.Key, option.Id, timeBackRunType.FullName);
                    return this;
                    //return BrunResultState.Success;
                }
                else
                {
                    throw new BrunException(BrunErrorCode.UnKnow, "the TimeWorker with key:'{0}' add TimeBackRun by id:'{1}' with type:'{2}' error,_backRuns.TryAdd return false.", this.Key, option.Id, timeBackRunType.FullName);
                    //_logger.LogError("the TimeWorker with key:'{0}' add TimeBackRun by id:'{1}' with type:'{2}' error,_backRuns.TryAdd return false.", this.Key, option.Id, timeBackRunType.FullName);
                    //return BrunResultState.UnKnow;
                }
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
