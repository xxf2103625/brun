using Brun;
using Brun.Services;
using BrunUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrunUI.Controllers
{
    public class PlanBrunController : BaseBrunController
    {
        IWorkerService workerService;
        IPlanBrunService planBrunService;
        public PlanBrunController(IWorkerService workerService, IPlanBrunService planBrunService)
        {
            this.workerService = workerService;
            this.planBrunService = planBrunService;
        }
        public InfoResult QueryList(int current,int pageSize)
        {
            var list = planBrunService.GetPlanBruns();
            int total = list.Count();
            var data = list.Skip(pageSize * (current - 1)).Take(pageSize).Select(m =>
            {
                PlanBackRun brun = (PlanBackRun)m.Value;
                return new BackRunInfoModel()
                {
                    Id = m.Key,
                    Name = m.Value.Name,
                    TypeName = m.Value.GetType().Name,
                    WorkerKey = m.Value.WorkerContext.Key,
                    WorkerName = m.Value.WorkerContext.Name,
                    StartTimes = m.Value.StartTimes,
                    ErrorTimes = m.Value.ErrorTimes,
                    EndTimes = m.Value.EndTimes,
                };
            });
            return InfoResult.Ok(new TableResult(data, total));
        }
    }
}
