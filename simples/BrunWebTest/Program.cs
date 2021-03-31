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
        public static string TimeKey = Guid.NewGuid().ToString();
        public static string ScopeKey = Guid.NewGuid().ToString();
        public static string PlanKey = Guid.NewGuid().ToString();
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
                    //��������
                    services.AddHttpClient();
                    services.AddScoped<ITestScopeService, TestScopeService>();

                    ////���õ�������
                    //WorkerBuilder.Create<TestHttpWorker>()
                    //.SetKey(BrunKey)
                    //.Build();

                    WorkerBuilder
                    .Create<LongTimeBackRun>()
                    .Add<LongTimeBackRun>()//ͬһ��OnceWorker�����ö��BackRun��ʹ�ã�worker.RunDontWait<TBackRun>()
                    .SetName(nameof(LongTimeBackRun))
                    .Build();

                    ////����Scope����
                    //WorkerBuilder.Create<TestScopeBackRun>()
                    //.SetKey(ScopeKey)
                    //.Build();


                    ////���ö�������
                    //WorkerBuilder.CreateQueue<TestQueueWorker>()
                    //.AddQueue<TestQueueErrorWorker>()//���ö��QueueBackRrun��ʹ��:worker.Enqueue<TQueueBackRun>(msg)
                    //.SetKey(QueueKey)
                    //.Build();


                    ////���ö�ʱ����
                    ////WorkerBuilder.CreateTime<ErrorTestRun>()
                    //WorkerBuilder.CreateTime<LongTimeBackRun>(TimeSpan.FromSeconds(5), true)
                    ////.SetCycle()
                    //.SetKey(TimeKey)
                    //.Build()
                    //;

                    //���ø���ʱ��ƻ�����
                    WorkerBuilder.CreatePlanTime<LogTimeRun>("0/5 * * * *", "3,33,53 * * * *", "5 * * * *", "* * * * *")
                    .AddPlanTime<ErrorTestRun>("* * * * *")
                    .SetKey(PlanKey)
                    .Build();

                    //������̨����
                    services.AddBrunService();
                })
                ;
    }
}
