using Brun;
using BrunUI.Auths;
using BrunUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Controllers
{
    /// <summary>
    /// Brun用户相关控制器
    /// </summary>
    public class UserController : BaseBrunController
    {
        IOptionsMonitor<BrunAuthenticationSchemeOptions> authOptions;
        public UserController(IOptionsMonitor<BrunAuthenticationSchemeOptions> brunAuthenticationSchemeOptions)
        {
            this.authOptions = brunAuthenticationSchemeOptions;
        }
        /// <summary>
        /// Burn登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public object Login(BrunLoginModel model)
        {
            HttpContext.Items.TryGetValue("BrunUser", out object testName);
            string t = (string)testName;
            //Console.WriteLine(t);
            //if (model.UserName != _workerServer.Option.UserName || model.Password != _workerServer.Option.Password)
            //{
            //    //账号或密码错误
            //    return new LoginResult()
            //    {
            //        Msg = "用户名或密码错误",
            //        Status = "error",
            //    };
            //}
            if (authOptions == null)
            {
                throw new Exception("_brunAuthenticationSchemeOptions is null");
            }
            if (authOptions.CurrentValue.UserName != model.UserName || authOptions.CurrentValue.Password != model.Password)
            {
                //账号或密码错误
                return new InfoResult(false,null, "用户名或密码错误");
            }
            BrunUser user = new BrunUser()
            {
                UserName = model.UserName,
                Password = model.Password,
                Roles = new List<string>() { "admin", "create" }
            };
            return new InfoResult(true, new LoginResult()
            {
                Msg = "登录成功",
                Status = "ok",
                Token = AesTokenHelper.GetToken(user)
            });
        }
        [HttpGet]
        public object CurrentUser()
        {
            //{"success":true,"data":{"name":"Serati Ma","avatar":"https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png","userid":"00000001","email":"antdesign@alipay.com","signature":"海纳百川，有容乃大","title":"交互专家","group":"蚂蚁金服－某某某事业群－某某平台部－某某技术部－UED","tags":[{"key":"0","label":"很有想法的"},{"key":"1","label":"专注设计"},{"key":"2","label":"辣~"},{"key":"3","label":"大长腿"},{"key":"4","label":"川妹子"},{"key":"5","label":"海纳百川"}],"notifyCount":12,"unreadCount":11,"country":"China","access":"admin","geographic":{"province":{"label":"浙江省","key":"330000"},"city":{"label":"杭州市","key":"330100"}},"address":"西湖区工专路 77 号","phone":"0752-268888888"}}
            return new InfoResult() { Success = true, Data = new { Name = User?.Identity?.Name, Roles = new List<string>() { "admin" }, Avatar = "https://gw.alipayobjects.com/zos/antfincdn/XAosXuNZyF/BiazfanxmamNRoxxVxka.png" } };
        }
        [HttpPost]
        public void OutLogin()
        {

        }
    }
}
