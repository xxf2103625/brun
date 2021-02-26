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
                IHttpClientFactory factory= GetRequiredService<IHttpClientFactory>();
                var fclient= factory.CreateClient();
                var fr= await fclient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                log.LogWarning("fclient发起了请求,state:" + fr.StatusCode);

                using (var scope = CreateScope())
                {
                    HttpClient iocClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
                    var iocR = await iocClient.GetAsync("http://127.0.0.1:5000/", stoppingToken);
                    log.LogWarning("iocClient发起了请求,state:" + iocR.StatusCode);
                }
            }
        }
    }
}
