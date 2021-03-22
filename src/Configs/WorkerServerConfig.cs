using Brun.Options;
using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brun
{
    public class WorkerServerConfig
    {
        public WorkerConfig DefaultConfig => new WorkerConfig();
        public WorkerOption DefaultOption => new WorkerOption();
        public TimeWorkerOption DefaultTimeWorkerOption => new TimeWorkerOption();
        public PlanTimeWorkerOption DefaultPlanTimeWorkerOption => new PlanTimeWorkerOption();
        public TimeSpan WaitDisposeOutTime { get; set; } = TimeSpan.FromSeconds(5);

        public List<Type> WorkerTypes = new List<Type>()
        {
            typeof(OnceWorker),
            typeof(SynchroWorker)
        };
    }
}
