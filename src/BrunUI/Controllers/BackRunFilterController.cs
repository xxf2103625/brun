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
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public List<string> GetOnceBackrun()
        {
            return filterService.GetOnceBackRunTypes().Select(m => m.Name).OrderBy(m => m).ToList();
        }
        public List<string> GetTimeBackrun()
        {
            return filterService.GetTimeBackRunTypes().Select(m => m.Name).OrderBy(m => m).ToList();
        }
        public List<string> GetQueueBackrun()
        {
            return filterService.GetQueueBackRunTypes().Select(m => m.Name).OrderBy(m => m).ToList();
        }
        public List<string> GetPlanBackrun()
        {
            return filterService.GetPlanBackRunTypes().Select(m => m.Name).OrderBy(m => m).ToList();
        }
    }
}
