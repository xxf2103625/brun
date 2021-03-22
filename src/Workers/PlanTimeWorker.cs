using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brun.BaskRuns;
using Brun.Commons;
using Brun.Enums;
using Brun.Options;
using Brun.Plan;
using Microsoft.Extensions.Logging;

namespace Brun.Workers
{
    /// <summary>
    /// 在计划时间内执行的Worker
    /// </summary>
    public class PlanTimeWorker : AbstractWorker, IPlanTimeWorker
    {
        //执行计划集合，type：backRun类型
        //一个PlanTime可能对应多个backrun
        private Dictionary<PlanTimeComputer, List<Type>> plans = new();
        private List<IBackRun> backRuns = new List<IBackRun>();
        private object backRunCreate_LOCK = new object();
        private ILogger logger => (ILogger<PlanTimeWorker>)WorkerServer.Instance.ServiceProvider.GetService(typeof(ILogger<PlanTimeWorker>));
        public PlanTimeWorker(PlanTimeWorkerOption option, WorkerConfig config) : base(option, config)
        {
            Init();
        }
        private void Init()
        {
            PlanTimeWorkerOption option = (PlanTimeWorkerOption)_option;
            foreach (var item in option.planTimeRuns)
            {
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
                            logger.LogWarning("planTime parse error:{0}", string.Join(",", planTime.Errors.Select(m => $"index {m.Key} msg {m.Value}")));
                        }
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
                    logger.LogWarning("the plantime {0} is allready has type {1}.", plan.Key, brunType.Name);
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
        public void Start()
        {
            foreach (var item in plans)
            {
                DateTimeOffset? nextTime = item.Key.GetNextTime();
                if (nextTime != null)
                {
                    item.Key.SetNextTime(nextTime.Value);
                }
            }
            Thread thread = new Thread(new ThreadStart(TimeListenning));
            thread.Start();
        }
        private async Task Execute(Type brunType)
        {
            await Observe(brunType, WorkerEvents.StartRun);
            try
            {
                IBackRun backRun = backRuns.FirstOrDefault(m => m.GetType() == brunType);
                if (backRun == null)
                {
                    lock (backRunCreate_LOCK)
                    {
                        if (backRun == null)
                        {
                            backRun = (IBackRun)BrunTool.CreateInstance(brunType);
                            backRuns.Add(backRun);
                        }
                    }
                }
                await backRun.Run(stoppingToken: tokenSource.Token);
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
        public void TimeListenning()
        {
            while (!tokenSource.IsCancellationRequested)
            {
                DateTime now = DateTime.Now;
                foreach (KeyValuePair<PlanTimeComputer, List<Type>> item in  plans)
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
                            RunningTasks.Add(TaskFactory.StartNew(() =>
                            {
                                Task task = Execute(bType);
                                task.ContinueWith(t => RunningTasks.TryTake(out t));
                            }));
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }
    }
}
