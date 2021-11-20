using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public BrunAuthenticationHandler(IOptionsMonitor<BrunAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var t = this.Context.User;
            var identity = new System.Security.Claims.ClaimsIdentity(authenticationType: "Brun", claims: new List<System.Security.Claims.Claim>()
                {
                    new System.Security.Claims.Claim("brun_name","brun_name_2"),
                    new System.Security.Claims.Claim("brun_role","admin,read_2"),
                });
            //AuthenticateResult.
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(identity), "Brun")));
        }
    }
    public class BrunAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {

    }
}
