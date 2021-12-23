using Brun.Redis;
using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public static class WorkerServerRedisOptionExtensions
    {
        public static WorkerServerOption UseRedis(this WorkerServerOption workerServerOption, string connString)
        {
            workerServerOption.StoreType = WorkerStoreType.Redis;
            workerServerOption.StoreConn = connString;
            workerServerOption.ServicesConfigure = services =>
            {
                //替换服务方法,暂时用不上
                //var descriptor =new ServiceDescriptor(typeof(),typeof(),ServiceLifetime.Scoped);
                //services.Replace()

                services.AddSingleton<ConnectionMultiplexer>(m => ConnectionMultiplexer.Connect(connString));
                //services.AddScoped<IOnceBrunService, OnceWorkerRedisService>();
            };
            return workerServerOption;
        }
    }
}
