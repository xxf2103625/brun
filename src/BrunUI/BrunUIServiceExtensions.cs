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
        public static IServiceCollection AddBrunUI(this IServiceCollection services)
        {
            services.AddAuthentication().AddScheme<BrunAuthenticationSchemeOptions, BrunAuthenticationHandler>("Brun", options =>
            {

            });
            return services;
        }
    }
}
