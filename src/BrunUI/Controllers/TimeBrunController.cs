using Brun.Services;
using BrunUI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrunUI.Controllers
{
    public class TimeBrunController: BaseBrunController
    {
        ITimeBrunService timeBrunService;
        public TimeBrunController(ITimeBrunService timeBrunService)
        {
            this.timeBrunService = timeBrunService;
        }
        [HttpGet]
        public TableResult QueryList(int current, int pageSize)
        {
            var list = timeBrunService.GetTimeBruns();
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
    }
}
