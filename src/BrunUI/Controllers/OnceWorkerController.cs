using Brun;
using Brun.BaskRuns;
using Brun.Commons;
using Brun.Models;
using Brun.Services;
using Brun.Workers;
using BrunUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Controllers
{
    /// <summary>
    /// 瞬时任务相关接口 TODO 重命名 OnceBrunController
    /// url: /brunapi/oncebrun/{action=Index}/{id?}
    /// </summary>
    public class OnceBrunController : BaseBrunController
    {
        IOnceBrunService onceBrunService;
        //IBaseWorkerService<OnceWorker> baseWorkerService;
        IWorkerService workerService;
        public OnceBrunController(IOnceBrunService onceBrunService, IWorkerService workerService)
        {
            this.onceBrunService = onceBrunService;
            //this.baseWorkerService = baseWorkerService;
            this.workerService = workerService;
        }
        [HttpGet]
        public async Task<TableResult> QueryList(int current, int pageSize)
        {
            var list = await onceBrunService.GetOnceBruns();
            int total = list.Count();
            var data = list.Skip(pageSize * (current - 1)).Take(pageSize).Select(m => new BackRunInfoModel()
            {
                Id = m.Key,
                Name = m.Value.Name,
                TypeName = m.Value.GetType().Name,
                WorkerKey = ((BackRun)m.Value).WorkerContext.Key,
                WorkerName = ((BackRun)m.Value).WorkerContext.Name,
            });
            return new TableResult(data, total);
        }
        [HttpPost]
        public async Task<BrunResultState> AddBrun(BrunCreateModel model)
        {
            var bType = BrunTool.GetTypeByFullName(model.BrunType);
            OnceBackRun? onceBrun =await onceBrunService.AddOnceBrun(model.WorkerKey, bType, new Brun.Options.OnceBackRunOption(model.Id, model.Name));
            if (onceBrun == null)
                return BrunResultState.Error;
            else
                return BrunResultState.Success;
        }
        [HttpPost]
        public async Task<BrunResultState> Run(BrunKeyModel model)
        {
            if (string.IsNullOrEmpty(model.BrunId))
                return BrunResultState.NotFound;
            var onceWorker = (OnceWorker)(await this.workerService.GetWorkerByKey(model.WorkerKey));
            if (onceWorker == null)
                return BrunResultState.NotFound;
            if (onceWorker.State != Brun.Enums.WorkerState.Started)
                return BrunResultState.NotRunning;
            onceWorker.Run(model.BrunId);
            return BrunResultState.Success;
        }
        /// <summary>
        /// url: /brunapi/onceworker/getonceworkersinfo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ValueLabel>> GetOnceWorkersInfo()
        {
            var workers = await workerService.GetAllOnceWorkers();
            return workers.Select(m => new ValueLabel(m.Key, m.Name));
        }
    }
}
