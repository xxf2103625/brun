﻿using Brun;
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
        IWorkerService workerService;
        IBackRunDetailService backRunDetailService;
        public OnceBrunController(IOnceBrunService onceBrunService, IWorkerService workerService, IBackRunDetailService backRunDetailService)
        {
            this.onceBrunService = onceBrunService;
            this.workerService = workerService;
            this.backRunDetailService = backRunDetailService;
        }
        [HttpGet]
        public  TableResult QueryList(int current, int pageSize)
        {
            var list = onceBrunService.GetOnceBruns();
            int total = list.Count();
            var data = list.Skip(pageSize * (current - 1)).Take(pageSize).Select(m => new BackRunInfoModel()
            {
                Id = m.Key,
                Name = m.Value.Name,
                TypeName = m.Value.GetType().Name,
                WorkerKey = m.Value.WorkerContext.Key,
                WorkerName = m.Value.WorkerContext.Name,
                StartTimes = m.Value.StartTimes,
                ErrorTimes = m.Value.ErrorTimes,
                EndTimes = m.Value.EndTimes,
            });
            return new TableResult(data, total);
        }
        [HttpPost]
        public  BrunResultState AddBrun(BrunCreateModel model)
        {
            var bType = BrunTool.GetTypeByFullName(model.BrunType);
            onceBrunService.AddOnceBrun(model.WorkerKey, bType, new OnceBackRunOption(model.Id, model.Name));
            return BrunResultState.Success;
        }
        [HttpPost]
        public BrunResultState Run(BrunKeyModel model)
        {
             onceBrunService.Run(model.BrunId);
            return BrunResultState.Success;
        }
        /// <summary>
        /// url: /brunapi/onceworker/getonceworkersinfo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ValueLabel> GetOnceWorkersInfo()
        {
            var workers = workerService.GetAllOnceWorkers();
            return workers.Select(m => new ValueLabel(m.Key, m.Name));
        }
        [HttpGet]
        public  BackRunContextNumberModel GetBrunDetailNumber(string brunId)
        {
            return  backRunDetailService.GetBackRunDetailNumber(brunId);
        }
    }
}