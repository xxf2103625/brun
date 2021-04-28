using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Options;
using Brun.Plan;
using Microsoft.Extensions.Logging;

namespace Brun.Workers
{
    /// <summary>
    /// 在计划时间执行的Worker
    /// </summary>
    public class PlanTimeWorker : AbstractWorker, IPlanTimeWorker
    {
        //执行计划集合，type：backRun类型 一个PlanTime可能对应多个backrun
        private Dictionary<PlanTimeComputer, List<Type>> plans = new Dictionary<PlanTimeComputer, List<Type>>();
        private List<IBackRun> backRuns = new List<IBackRun>();
        private object backRunCreate_LOCK = new object();
        //private ILogger Logger => (ILogger<PlanTimeWorker>)WorkerServer.Instance.ServiceProvider.GetService(typeof(ILogger<PlanTimeWorker>));
        //初始化所有类型
        public override IEnumerable<Type> BrunTypes => backRuns.Select(m => m.GetType());

        public PlanTimeWorker(PlanTimeWorkerOption option, WorkerConfig config) : base(option, config)
        {
            Init();
        }
        private void Init()
        {
            PlanTimeWorkerOption option = (PlanTimeWorkerOption)_option;
            foreach (KeyValuePair<Type, List<string>> item in option.planTimeRuns)
            {
                if (!backRuns.Any(m => m.GetType() == item.Key))
                {
                    var brun = (IBackRun)BrunTool.CreateInstance(item.Key);
                    brun.Data = _context.Items;
                    backRuns.Add(brun);
                }
                if (item.Value != null && item.Value.Count > 0)
                {
                    for (int i = 0; i < item.Value.Count; i++)
                    {
                        PlanTime planTime = new PlanTime();
                        if (planTime.Parse(item.Value[i]))
                        {
                            AddBrun(planTime, item.Key);
                        }
                        else
                        {
                            Logger.LogWarning("planTime parse error:{0}", string.Join(",", planTime.Errors.Select(m => $"index {m.Key} msg {m.Value}")));
                        }
                    }
                }
            }
            foreach (var item in plans)
            {
                if (item.Key.NextTime == null)
                {
                    DateTimeOffset? nextTime = item.Key.GetNextTime();
                    if (nextTime != null)
                    {
                        item.Key.SetNextTime(nextTime.Value);
                    }
                }
            }
        }
        private void AddBrun(PlanTime planTime, Type brunType)
        {
            if (planTime == null || !planTime.IsSuccess)
            {
                throw new Exception("PlanTime is null or error.");
            }
            if (plans.Any(m => m.Key.PlanTime.Expression == planTime.Expression))
            {
                var plan = plans.First(m => m.Key.PlanTime.Expression == planTime.Expression);
                if (plan.Value.Contains(brunType))
                {
                    Logger.LogWarning("the plantime {0} is allready has type {1}.", plan.Key, brunType.Name);
                }
                else
                {
                    plan.Value.Add(brunType);
                }
            }
            else
            {
                var plan = new PlanTimeComputer(planTime);
                plans[plan] = new List<Type>() { brunType };
            }
        }
        public override void Start()
        {
            if (_context.State == WorkerState.Started)
            {
                Logger.LogWarning("the {0} key:{1} is already started.", GetType().Name, Key);
                //return Task.CompletedTask;
            }
            _context.State = WorkerState.Started;
            //TODO 减少不必要的多线程开销
            Task.Factory.StartNew(() =>
             {
                 while (!tokenSource.IsCancellationRequested && _context.State == WorkerState.Started)
                 {
                     DateTime now = DateTime.Now;
                     foreach (KeyValuePair<PlanTimeComputer, List<Type>> item in plans)
                     {
                         if (item.Value != null && item.Value.Count > 0 && item.Key.NextTime != null && now > item.Key.NextTime.Value)
                         {
                             DateTimeOffset? next = item.Key.GetNextTime(now);
                             if (next != null)
                             {
                                 item.Key.SetNextTime(next.Value);
                             }
                             item.Key.SetLastTime(now);

                             foreach (Type bType in item.Value)
                             {
                                 BrunContext brunContext = new BrunContext(bType);
                                 Task.Run(async () =>
                                 {
                                     await Execute(brunContext);
                                 });
                             }
                         }
                     }
                     Thread.Sleep(5);
                 }
             }, creationOptions: TaskCreationOptions.LongRunning);
        }
        protected override async Task Brun(BrunContext context)
        {
            await GetBackRun(context).Run(tokenSource.Token);
        }
        public override void Dispose()
        {
            //首先停止监听，避免每秒执行时卡主进程
            Stop();
            base.Dispose();
        }
        private IBackRun GetBackRun(BrunContext brunContext)
        {
            IBackRun backRun = backRuns.FirstOrDefault(m => m.GetType() == brunContext.BrunType);
            if (backRun == null)
            {
                lock (backRunCreate_LOCK)
                {
                    if (backRun == null)
                    {
                        backRun = (IBackRun)BrunTool.CreateInstance(brunContext.BrunType);
                        backRuns.Add(backRun);
                    }
                }
            }
            return backRun;
        }
    }
}
