using Brun.Models;
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
    /// 查询Backrun接口
    /// url: /brunapi/backrunfilter/{action=Index}/{id?}
    /// </summary>
    public class BackRunFilterController : BaseBrunController
    {
        IBackRunFilterService filterService;
        public BackRunFilterController(IBackRunFilterService filterService)
        {
            this.filterService = filterService;
        }
        //url: /brunapi/backrunfilter/getoncebackrun
        [HttpGet]
        public List<ValueLabel> GetOnceBackrun()
        {
            return filterService.GetOnceBackRunTypes().OrderBy(m => m.Name).Select(m =>new ValueLabel(m.FullName,m.Name)).ToList();
        }
        [HttpGet]
        public List<ValueLabel> GetTimeBackrun()
        {
            return filterService.GetTimeBackRunTypes().OrderBy(m=>m.Name).Select(m => new ValueLabel(m.FullName, m.Name)).ToList();
        }
        [HttpGet]
        public List<ValueLabel> GetQueueBackrun()
        {
            return filterService.GetQueueBackRunTypes().OrderBy(m => m.Name).Select(m => new ValueLabel(m.FullName, m.Name)).OrderBy(m => m).ToList();
        }
        [HttpGet]
        public List<ValueLabel> GetPlanBackrun()
        {
            return filterService.GetPlanBackRunTypes().OrderBy(m => m.Name).Select(m => new ValueLabel(m.FullName, m.Name)).ToList();
        }
    }
}
