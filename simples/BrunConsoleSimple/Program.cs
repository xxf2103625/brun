using Brun;
using Brun.Commons;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunConsoleSimple
{
    class Program
    {
        static string GetNow => DateTime.Now.ToString("HH:mm:ss ffff");
        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }
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
                services.AddBrunService(workerServer =>
                {

                });
            });
    }
}
