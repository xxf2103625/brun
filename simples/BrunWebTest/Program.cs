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
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    //��������
                    services.AddHttpClient();
                    //��������
                    WorkerBuilder.Create<TestHttpWorker>()
                    .SetKey(BrunKey)
                    .Build();
                    //������̨����
                    services.AddBrunService();
                })
                ;
    }
}