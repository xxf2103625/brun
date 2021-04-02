using Brun.BaskRuns;
using Brun.Contexts;
using Brun.Enums;
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
    /// 简单的时间循环任务，复杂的定时使用<see cref="PlanTimeWorker"/>
    /// //TODO 最简易循环执行任务，继续简化使用
    /// </summary>
    public class TimeWorker : AbstractWorker, ITimeWorker
    {
        private TimeWorkerOption TimeOption => (TimeWorkerOption)_option;
        public override IEnumerable<Type> BrunTypes => TimeOption.CycleTimes.Select(m => m.BackRun.GetType());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="config"></param>
        public TimeWorker(TimeWorkerOption option, WorkerConfig config) : base(option, config)
        {
            Init();
        }
        private void Init()
        {
            foreach (var item in TimeOption.CycleTimes)
            {
                if (item.RunWithStart)
                {
                    item.NextTime = DateTime.Now;
                }
                else
                {
                    item.NextTime = DateTime.Now.Add(item.Cycle);
                }
            }
        }
        /// <summary>
        /// 实例内保持唯一
        /// </summary>
        protected IBackRun GetBackRun(Type brunType)
        {
            SimpleCycleTime cycleTime = TimeOption.CycleTimes.FirstOrDefault(m => m.BackRun.GetType() == brunType);
            if (cycleTime == null)
            {
                Logger.LogWarning("the {0}'s {1} is not init.", GetType(), brunType.Name);
            }
            return cycleTime.BackRun;
        }
        /// <summary>
        /// 启动Worker
        /// </summary>
        /// <returns></returns>
        public override Task Start()
        {
            if (_context.State != WorkerState.Started)
            {
                _context.State = WorkerState.Started;
                Task.Factory.StartNew(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested && _context.State == WorkerState.Started)
                    {
                        foreach (var item in TimeOption.CycleTimes)
                        {
                            if (item.NextTime != null && item.NextTime.Value <= DateTime.Now)
                            {
                                BrunContext brunContext = new BrunContext(item.BackRun.GetType());
                                _ = Execute(brunContext);
                                item.NextTime = DateTime.Now.Add(item.Cycle);
                            }
                        }
                        Thread.Sleep(5);
                    }
                }, TaskCreationOptions.LongRunning);
                Logger.LogInformation("the {0} key:{1} is started", GetType().Name, _context.Key);
                return Task.CompletedTask;
            }
            Logger.LogWarning("the TimeWorker key:{0} is already started.", _context.Key);
            return Task.CompletedTask;
        }
        protected override Task Brun(BrunContext context)
        {
            return GetBackRun(context.BrunType).Run(tokenSource.Token);
        }
    }
}
