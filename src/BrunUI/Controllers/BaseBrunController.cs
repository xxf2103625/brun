using BrunUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Controllers
{
    /// <summary>
    /// BrunUI组件接口控制器基类
    /// </summary>
    [ApiController]
    [TypeFilter(typeof(BrunExceptionFilter))]
    [Authorize(AuthenticationSchemes = "Brun")]
    [Route(template: "/brunapi/{controller=Home}/{action=Index}/{id?}")]
    public class BaseBrunController : ControllerBase
    {

    }
}
