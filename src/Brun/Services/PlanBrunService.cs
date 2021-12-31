using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Services
{
    public class PlanBrunService : IPlanBrunService
    {
        public Task<IPlanWorker> AddPlanBrun(IPlanWorker planWorker, Type brunType, PlanBackRunOption option)
        {
            return Task.FromResult(((PlanWorker)planWorker).ProtectAddBrun(brunType, option));
        }
        public Task<IPlanWorker> AddPlanBrun<TPlanBackRun>(IPlanWorker planWorker, PlanBackRunOption option) where TPlanBackRun : PlanBackRun
        {
            return this.AddPlanBrun(planWorker, typeof(TPlanBackRun), option);
        }
    }
}
