using System;
using System.Collections.Generic;
using System.Threading;
using Brun.Options;
using Brun.Plan;

namespace Brun.Workers
{
    /// <summary>
    /// 在计划时间内执行的Worker
    /// </summary>
    public class PlanTimeWorker : AbstractWorker, IWorker
    {
        //时间计划
        private List<PlanTimeComputer> planTimeComputers = new List<PlanTimeComputer>();

        public PlanTimeWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {

        }
        public void Run()
        {

        }
        public void Start()
        {

        }
        public void TimeListenning()
        {
            while (!tokenSource.IsCancellationRequested)
            {
                foreach (var item in planTimeComputers)
                {
                    if (item.NextTime != null && DateTime.Now >= item.NextTime)
                    {

                    }
                }
                Thread.Sleep(5);
            }

        }
    }
}
