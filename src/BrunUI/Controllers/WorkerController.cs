using Brun.Models;
using Brun.Services;
using BrunUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Controllers
{
    public class WorkerController : BaseBrunController
    {
        IWorkerService workerService;
        public WorkerController(IWorkerService workerService)
        {
            this.workerService = workerService;
        }
        [HttpPost]
        public async Task<BrunResultState> AddWorker(WorkerModel model)
        {

            return await workerService.AddWorker(new WorkerConfigModel() { Key = model.Key, Name = model.Name }, model.WorkerType);
        }
        [HttpGet]
        public TableResult GetWorkers(int current, int pageSize)
        {
            var data = workerService.GetWorkerInfos().OrderBy(m => m.Name).Skip(pageSize * (current - 1)).Take(pageSize).ToList();
            return new TableResult()
            {
                Data = data,
                Total = workerService.GetWorkerInfos().Count(),
                Success = true
            };
        }
    }
}
