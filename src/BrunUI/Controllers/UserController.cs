using Brun;
using BrunUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Controllers
{
    
    public class UserController : BaseBrunController
    {
        private WorkerServer _workerServer;
        public UserController(WorkerServer workerServer)
        {
            _workerServer = workerServer;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(BrunLoginModel model)
        {
            HttpContext.Items.TryGetValue("BrunUser", out object? testName);
            string? t=(string?)testName;
            Console.WriteLine(t);
            if (model.UserName != _workerServer.Option.UserName || model.Password != _workerServer.Option.Password)
            {
                //账号或密码错误
                return Json(new { Msg = "用户名或密码错误" });
            }
            return Json(new { Msg = "登录成功" });
        }
    }
}
