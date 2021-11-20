using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Controllers
{
    public class BrunController : BaseBrunController
    {
        public IActionResult Index()
        {
            return Content("brun controller");
        }
        [HttpPost]
        public IActionResult Login()
        {
            return Json(new
            {
                Name = "admin",
                Role = new string[] { "admin", "read", "update", "delete" }
            }); ;
        }
        public IActionResult CurrentUser()
        {
            return Json(new
            {
                Name = "admin",
                Role = "admin"
            });
        }
    }
}
