using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Exceptions;
using Brun.Options;
using Brun.Plan;
using Microsoft.Extensions.Logging;

namespace Brun.Workers
{
    /// <summary>
    /// 在计划时间执行的Worker
    /// </summary>
    public class PlanWorker : AbstractWorker, IPlanTimeWorker
    {
        //计算计划时间的工具类
        private PlanTimeComputer planTimeComputer;

        public PlanWorker(WorkerConfig config) : base(config)
        {
            Init();
        }
        private void Init()
        {
            planTimeComputer = new PlanTimeComputer();
            if (string.IsNullOrEmpty(this._config.Key))
            {
                this._config.Key = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(this._config.Name))
            {
                this._config.Name = nameof(PlanWorker);
            }
        }

        public override void Start()
        {
            if (_context.State == WorkerState.Started)
            {
                _logger.LogWarning("the {0} key:'{1}' is already started.", GetType().Name, Key);
                return;
            }
            _context.State = WorkerState.Started;
            //TODO 减少不必要的多线程开销
            Task.Factory.StartNew(() =>
             {
                 while (!tokenSource.IsCancellationRequested && _context.State == WorkerState.Started)
                 {
                     DateTime now = DateTime.Now;
                     foreach (var item in _backRuns)
                     {
                         PlanBackRun backRun = (PlanBackRun)item.Value;
                         if (backRun.LastRunTime == null)
                         {
                             if (backRun.Option.PlanTime.Begin > now)
                             {
                                 //还没到开始执行时间,跳过此backrun
                                 continue;
                             }
                             //
                         }
                         if (backRun.NextRunTime == null)
                         {
                             var nextRunTime = planTimeComputer.GetNextTime(backRun.Option.PlanTime,now);
                             if (nextRunTime == null)
                             {
                                 _logger.LogError("the {0} in PlanWorker with id:'{1}' nextRunTime is null,delete this PlanWorker.", backRun.GetType(), backRun.Id);
                                 this._backRuns.TryRemove(backRun.Id, out _);
                             }
                             backRun.NextRunTime = nextRunTime;
                             continue;//下一轮再判断
                         }
                         if (backRun.NextRunTime < now)
                         {
                             backRun.LastRunTime = now;
                             backRun.NextRunTime = planTimeComputer.GetNextTime(backRun.Option.PlanTime,now);
                             if (backRun.NextRunTime == null)
                             {
                                 _logger.LogWarning("the {0} in PlanWorker with id:'{1}' nextRunTime is null,delete this PlanWorker.", backRun.GetType(), backRun.Id);
                                 //下一轮会移除
                             }
                             BrunContext brunContext = new BrunContext(backRun);
                             Task.Run(async () =>
                             {
                                 await Execute(brunContext);
                             });
                         }
                     }
                     Thread.Sleep(5);
                 }
             }, creationOptions: TaskCreationOptions.LongRunning);
        }
        protected override Task Brun(BrunContext context)
        {
            PlanBackRun backrun = (PlanBackRun)context.BackRun;
            backrun.SetWorkerContext(_context);
            return backrun.Run(tokenSource.Token);
        }
        public override void Dispose()
        {
            //首先停止监听，避免每秒执行时卡主进程
            Stop();
            base.Dispose();
        }
        public PlanWorker AddBrun(Type planBackRunType, PlanBackRunOption option)
        {
            if (!planBackRunType.IsSubclassOf(typeof(PlanBackRun)))
            {
                throw new BrunTypeErrorException($"{planBackRunType.FullName} can not add to PlanWorker.");
            }
            if (_backRuns.Any(m => m.Key == option.Id))
            {
                _logger.LogError("the PlanWorker key:'{0}' has allready added PlanBackRun by id:'{1}' with type:'{2}'.", this.Key, option.Id, planBackRunType.FullName);
                return this;
            }
            else
            {
                PlanBackRun planBackRun = (PlanBackRun)BrunTool.CreateInstance(planBackRunType, option);
                _backRuns.TryAdd(planBackRun.Id, planBackRun);
                //InitPreTimeBackRun(queueBackRun);
                _logger.LogInformation("the PlanWorker with key:'{0}' added PlanBackRun by id:'{1}' with type:'{2}' success.", this.Key, option.Id, planBackRunType.FullName);
                return this;
            }
        }
    }
}
