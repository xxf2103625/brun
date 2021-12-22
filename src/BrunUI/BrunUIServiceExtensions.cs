using BrunUI.Auths;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI
{
    public static class BrunUIServiceExtensions
    {
        public static IServiceCollection AddBrunUI(this IServiceCollection services, Action<BrunAuthenticationSchemeOptions>? configrureOptions = null)
        {
            services.Configure<BrunAuthenticationSchemeOptions>(configrureOptions);
            services.AddAuthentication().AddScheme<BrunAuthenticationSchemeOptions, BrunAuthenticationHandler>("Brun", configrureOptions);//configrureOptions在仅BrunAuthenticationHandler中生效
            return services;
        }
    }
}
