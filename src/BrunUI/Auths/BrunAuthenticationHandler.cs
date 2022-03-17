using BrunUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BrunUI.Auths
{
    /// <summary>
    /// BrunApi接口权限Handler
    /// </summary>
    public class BrunAuthenticationHandler : AuthenticationHandler<BrunAuthenticationSchemeOptions>
    {
        IOptionsMonitor<BrunAuthenticationSchemeOptions> _options;
        public BrunAuthenticationHandler(IOptionsMonitor<BrunAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _options = options;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (_options.CurrentValue.AuthType == AuthType.SimpleToken)
            {
                if (this.Context.Request.Headers.TryGetValue(_options.CurrentValue.HeadName, out Microsoft.Extensions.Primitives.StringValues tokenValues))
                {
                    string token = tokenValues.ToString();
                    if (token.Length > 0)
                    {
                        var user = AesTokenHelper.GetUser(token, _options.CurrentValue.BrunSimpleTokenKey);
                        if (user != null)
                        {
                            if (_options.CurrentValue.UserName == user.UserName && _options.CurrentValue.Password == user.Password)
                            {
                                var identity = new ClaimsIdentity(authenticationType: "Brun", claims: new List<Claim>()
                                {
                                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                                    new Claim(ClaimTypes.Name,user.UserName),
                                });
                                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), "Brun")));
                            }
                        }
                    }
                }
            }
            return Task.FromResult(AuthenticateResult.Fail("无权访问"));
        }
    }
    public class BrunAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// 认证类型
        /// </summary>
        public AuthType AuthType { get; set; }
        /// <summary>
        /// 前端head传入token的key值
        /// </summary>
        public string HeadName { get; set; } = "brun_auth";
        /// <summary>
        /// Brun简单Token认证的密钥，必须32位，aes对称加密，不要泄露
        /// </summary>
        public string BrunSimpleTokenKey { get; set; } = "mvyyybozkaairhpfwmievusfmjndhzcg";

        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "admin";
    }
}
