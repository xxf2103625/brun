using Brun;
using Brun.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestBrun
{
    [TestClass]
    public class DefaultBackgroundServiceTest
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestMethod1Async()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            //services.AddMyServices();
                            services.AddHostedService<BrunBackgroundService>();
                        })
                        .Configure(app =>
                        {
                            //app.UseMiddleware<MyMiddleware>();
                        })
                        ;
                        })
                .StartAsync();
            await host.GetTestClient().GetAsync("/");
            await host.StopAsync();
        }
    }
}
