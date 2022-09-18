using Brun.BaskRuns;
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
        IWorkerService workerService;
        public PlanBrunService(IWorkerService workerService)
        {
            this.workerService = workerService;
        }
        public IPlanWorker AddPlanBrun(IPlanWorker planWorker, Type brunType, PlanBackRunOption option)
        {
            return ((PlanWorker)planWorker).ProtectAddBrun(brunType, option);
        }
        public IPlanWorker AddPlanBrun<TPlanBackRun>(IPlanWorker planWorker, PlanBackRunOption option) where TPlanBackRun : PlanBackRun
        {
            return this.AddPlanBrun(planWorker, typeof(TPlanBackRun), option);
        }
        public IEnumerable<KeyValuePair<string, IBackRun>> GetPlanBruns()
        {
            var result = new List<KeyValuePair<string, IBackRun>>();
            var workers = workerService.GetAllPlanWorkers();
            foreach (PlanWorker item in workers)
            {
                foreach (var brun in item.BackRuns)
                {
                    result.Add(brun);
                }
            }
            return result;
        }
    }
}
