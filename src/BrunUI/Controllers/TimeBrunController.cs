using Brun;
using Brun.Models;
using Brun.Services;
using BrunUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrunUI.Controllers
{
    public class TimeBrunController : BaseBrunController
    {
        IWorkerService workerService;
        ITimeBrunService timeBrunService;
        public TimeBrunController(ITimeBrunService timeBrunService, IWorkerService workerService)
        {
            this.timeBrunService = timeBrunService;
            this.workerService = workerService;
        }
        [HttpGet]
        public InfoResult QueryList(int current, int pageSize)
        {
            var list = timeBrunService.GetTimeBruns();
            int total = list.Count();
            var data = list.Skip(pageSize * (current - 1)).Take(pageSize).Select(m =>
            {
                TimeBackRun brun = (TimeBackRun)m.Value;
                double cycle= brun.Option.Cycle.TotalSeconds;
                return new TimeBackRunInfoModel()
                {
                    Id = m.Key,
                    Name = m.Value.Name,
                    TypeName = m.Value.GetType().Name,
                    WorkerKey = m.Value.WorkerContext.Key,
                    WorkerName = m.Value.WorkerContext.Name,
                    StartTimes = m.Value.StartTimes,
                    ErrorTimes = m.Value.ErrorTimes,
                    EndTimes = m.Value.EndTimes,
                    TotalSeconds=cycle,
                };
            });
            return InfoResult.Ok(new TableResult(data, total));
        }
        [HttpGet]
        public InfoResult GetOnceWorkersInfo()
        {
            var workers = workerService.GetAllTimeWorkers();
            return InfoResult.Ok( workers.Select(m => new ValueLabel(m.Key, m.Name)));
        }
    }
}
