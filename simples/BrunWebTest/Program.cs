using Brun;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrunWebTest
{
    public class Program
    {
        public static string BrunKey = Guid.NewGuid().ToString();
        public static string QueueKey = Guid.NewGuid().ToString();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    //其他服务
                    services.AddHttpClient();
                    services.AddScoped<ITestScopeService, TestScopeService>();

                    //配置单次任务
                    WorkerBuilder.Create<TestHttpWorker>()
                    .SetKey(BrunKey)
                    .Build();

                    //配置队列任务
                    WorkerBuilder.CreateQueue<TestQueueWorker>()
                    .SetKey(QueueKey)
                    .Build();

                    //启动后台服务
                    services.AddBrunService();
                })
                ;
    }
}
