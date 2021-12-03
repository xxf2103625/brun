using Brun.Services;
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
        public OnceWorkerController(IOnceWorkerService onceWorkerService)
        {
            this.onceWorkerService = onceWorkerService;
        }
        [HttpGet]
        public async Task<object> QueryList(int current, int pageSize)
        {
            var list = await onceWorkerService.GetOnceBruns();
            //return list;
            return new { Data = list, Total = list.Count(), Success = true, current = current, pageSize = pageSize };
        }
    }
}
