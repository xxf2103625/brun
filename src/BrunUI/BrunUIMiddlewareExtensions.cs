using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI
{
    public static class BrunUIMiddlewareExtensions
    {
        public static IApplicationBuilder UseBrunUI(
            this IApplicationBuilder builder)
        {
            return builder.Map("/brun", app =>
            {
                app.UseMiddleware<BrunUIMiddleware>();
            });
        }
    }
}
