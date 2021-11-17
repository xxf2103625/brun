using Brun;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BrunWebTest
{
    public class LogTimeRun : BackRun
    {
        public override async Task Run(CancellationToken stoppingToken)
        {
            GetRequiredService<ILogger<LogTimeRun>>().LogInformation("{0} this is logTimeRun", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss FFFF"));
            await Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
    public class ErrorTestRun : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            throw new NotImplementedException("测试异常");
        }
    }
    public class TestHttpWorker : BackRun
    {
        public async override Task Run(CancellationToken stoppingToken)
        {

            for (int i = 0; i < 1; i++)
            {
                var log = GetRequiredService<ILogger<TestHttpWorker>>();

                HttpClient httpClient = new HttpClient();
                var r = await httpClient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                log.LogWarning("httpClient发起了请求,state:" + r.StatusCode);

                //也能使用自带的ioc获取服务，瞬时和单例可以直接取，Scoped的需要自己创建CreateScope
                IHttpClientFactory factory = GetRequiredService<IHttpClientFactory>();
                var fclient = factory.CreateClient();
                var fr = await fclient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                log.LogWarning("fclient发起了请求,state:" + fr.StatusCode);

                using (var scope = NewScope())
                {
                    var testScope = scope.ServiceProvider.GetRequiredService<ITestScopeService>();
                    string scopeR = testScope.Todo();
                    log.LogInformation(scopeR);
                }
            }
        }
    }
    public class TestQueueWorker : QueueBackRun
    {
        public TestQueueWorker(QueueBackRunOption option) : base(option)
        {
        }

        public override Task Run(string message, CancellationToken stoppingToken)
        {

            var log = GetRequiredService<ILogger<TestQueueWorker>>();
            if (message == "2")
            {
                throw new NotSupportedException("不支持2");
            }
            else
            {
                log.LogInformation("接收到消息:{0}", message);
            }
            //await Task.Delay(TimeSpan.FromSeconds(2));
            return Task.CompletedTask;
        }
    }
    public class TestQueueErrorWorker : QueueBackRun
    {
        public TestQueueErrorWorker(QueueBackRunOption option) : base(option)
        {
        }

        public override Task Run(string message, CancellationToken stoppingToken)
        {
            throw new NotImplementedException("请自己实现");
        }
    }
}
