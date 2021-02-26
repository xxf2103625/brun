using Brun;
using Brun.Commons;
using BrunTestHelper.BackRuns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace BrunConsoleSimple
{
    class Program
    {
        static void Main(string[] args)
        {

            IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }
        /*
         框架提供的服务
        自动注册以下服务：
            IHostApplicationLifetime
            IHostLifetime
            IHostEnvironment / IWebHostEnvironment
         */
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
            })
            .ConfigureHostConfiguration(m =>
            {
            })
            .ConfigureServices((hostContext, services) =>
            {
                //services.AddLogging(logConfigure =>
                //{

                //});
                services.AddBrunService();
                //services.AddHostedService<BrunBackgroundService>();
            });
    }
}
