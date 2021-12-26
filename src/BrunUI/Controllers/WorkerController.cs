using Brun;
using Brun.Commons;
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
            Type type = BrunTool.GetWorkerType(model.WorkerType);
            if (model.Key == null)
            {
                model.Key = Guid.NewGuid().ToString();
            }
            if (model.Name == null)
            {
                model.Name = type.Name;
            }
            var wk = await workerService.AddWorkerAndStart(new WorkerConfig() { Key = model.Key, Name = model.Name }, type);
            if (wk != null)
                return BrunResultState.Success;
            else
                return BrunResultState.UnKnow;
        }
        [HttpGet]
        public async Task<TableResult> GetWorkers(int current, int pageSize)
        {
            var data = await workerService.GetWorkerInfos(current, pageSize);
            return new TableResult(data.Item1, data.Item2);
        }
    }
}
