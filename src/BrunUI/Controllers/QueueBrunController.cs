using Brun;
using Brun.Services;
using BrunUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrunUI.Controllers
{
    public class QueueBrunController : BaseBrunController
    {
        IWorkerService workerService;
        IQueueBrunService queueBrunService;
        public QueueBrunController(IWorkerService workerService, IQueueBrunService queueBrunService)
        {
            this.workerService = workerService;
            this.queueBrunService = queueBrunService;
        }
        [HttpGet]
        public InfoResult QueryList(int current, int pageSize)
        {
            var list = queueBrunService.GetQueueBruns();
            int total = list.Count();
            var data = list.Skip(pageSize * (current - 1)).Take(pageSize).Select(m =>
            {
                QueueBackRun brun = (QueueBackRun)m.Value;
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
