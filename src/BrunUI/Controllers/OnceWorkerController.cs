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
    /// 瞬时任务相关接口
    /// url: /brunapi/onceworker/{action=Index}/{id?}
    /// </summary>
    public class OnceWorkerController : BaseBrunController
    {
        IOnceWorkerService onceWorkerService;
        IBaseWorkerService<OnceWorker> baseWorkerService;
        public OnceWorkerController(IOnceWorkerService onceWorkerService, IBaseWorkerService<OnceWorker> baseWorkerService)
        {
            this.onceWorkerService = onceWorkerService;
            this.baseWorkerService = baseWorkerService;
        }
        [HttpGet]
        public TableResult QueryList(int current, int pageSize)
        {
            //TODO 移到service里
            int total = onceWorkerService.GetOnceBruns().Count();
            var onceWorkers = baseWorkerService.GetWorkers().ToList();
            int index = 0;
            int start = (current - 1) * pageSize;
            int end = current * pageSize;
            var r = new List<BackRunInfoModel>();
            for (int i = 0; i < onceWorkers.Count; i++)
            {
                for (int n = 0; n < onceWorkers[i].BackRuns.Count; n++)
                {
                    index++;
                    if (start < index && index <= end)
                    {
                        r.Add(new BackRunInfoModel()
                        {
                            Id = onceWorkers[i].BackRuns.ElementAt(n).Value.Id,
                            Name = onceWorkers[i].BackRuns.ElementAt(n).Value.Name,
                            TypeName = onceWorkers[i].BackRuns.ElementAt(n).Value.GetType().Name,
                            TypeFullName = onceWorkers[i].BackRuns.ElementAt(n).Value.GetType().FullName,
                            WorkerKey = onceWorkers[i].Key,
                            WorkerName = onceWorkers[i].Name

                        });
                    }
                    if (index > end)
                    {
                        break;
                    }
                }
            }
            //var list = onceWorkerService.GetOnceBruns().Skip(pageSize * (current - 1)).Take(pageSize).Select(m => new BackRunInfoModel()
            //{
            //    Id = m.Key,
            //    TypeName = m.Value.GetType().Name,
            //});
            //return list;
            return new TableResult(r, total, true);
            //return new { Data = list, Total = total, Success = true, current = current, pageSize = pageSize };
        }
        [HttpPost]
        public BrunResultState AddBrun(BrunCreateModel model)
        {
            var onceWorker = this.baseWorkerService.GetWorkerByKey(model.WorkerKey);
            if (onceWorker == null)
                return BrunResultState.NotFound;
            Type bType = Brun.Commons.BrunTool.GetTypeByFullName(model.BrunType);
            if (bType == null)
                return BrunResultState.NotFound;
            return onceWorker.AddBrun(bType, new Brun.Options.OnceBackRunOption(model.Id, model.Name));
            //return BrunResultState.Success;
        }
        [HttpPost]
        public BrunResultState Run(BrunKeyModel model)
        {
            var onceWorker = this.baseWorkerService.GetWorkerByKey(model.WorkerKey);
            if (onceWorker == null)
                return BrunResultState.NotFound;
            if (onceWorker.State != Brun.Enums.WorkerState.Started)
                return BrunResultState.NotRunning;
            //Type bType = Brun.Commons.BrunTool.GetTypeByFullName(model.BrunType);
            if (string.IsNullOrEmpty(model.BrunId))
                return BrunResultState.NotFound;
            onceWorker.Run(model.BrunId);
            return BrunResultState.Success;
        }
        /// <summary>
        /// url: /brunapi/onceworker/getonceworkersinfo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ValueLabel> GetOnceWorkersInfo()
        {
            return onceWorkerService.GetOnceWorkersInfo();
        }
    }
}
