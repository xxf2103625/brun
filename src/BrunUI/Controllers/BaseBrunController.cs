using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize(AuthenticationSchemes = "Brun")]
    [Route(template: "/brunapi/{controller=Home}/{action=Index}/{id?}")]
    public class BaseBrunController : Controller
    {

    }
}
