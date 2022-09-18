using BrunUI.Auths;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI
{
    public static class BrunUIServiceExtensions
    {
        public static IServiceCollection AddBrunUI(this IServiceCollection services, Action<BrunAuthenticationSchemeOptions> configrureOptions = null)
        {
            initDistFiles();
            services.Configure<BrunAuthenticationSchemeOptions>(configrureOptions);
            services.AddAuthentication().AddScheme<BrunAuthenticationSchemeOptions, BrunAuthenticationHandler>("Brun", configrureOptions);//configrureOptions在仅BrunAuthenticationHandler中生效
            return services;
        }

        private static void initDistFiles()
        {
            var assembly = typeof(BrunUIMiddleware).Assembly;
            var names= assembly.GetManifestResourceNames();
            if (names == null)
            {
                throw new FileNotFoundException("not found 'dist' static files");
            }
            for (int i = 0; i < names.Length; i++)
            {
                var stream = assembly.GetManifestResourceStream(names[i]);
                byte[] bts = new byte[stream.Length];
                stream.Read(bts, 0, (int)stream.Length);
                BrunUIMiddleware.DistFiles.Add(names[i],bts);
            }
        }
    }
}
