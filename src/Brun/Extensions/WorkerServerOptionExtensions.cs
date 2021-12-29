using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public static class WorkerServerOptionExtensions
    {
        public static WorkerServerOption UseInMemory(this WorkerServerOption workerServerOption)
        {
            workerServerOption.StoreType = WorkerStoreType.Memory;
            workerServerOption.ServicesConfigure = services =>
            {
                //操作worker相关服务
                services.AddScoped<IWorkerService, WorkerService>();
                //操作OnceBackRun相关服务
                services.AddScoped<IOnceBrunService, OnceBrunService>();
                //查询BackRun运行时信息相关服务
                services.AddScoped<IBackRunDetailService, BackRunDetailService>();


                //记录BackRun运行时信息相关服务,Client端不需要使用这个，单例减少创建Ioc'Scope消耗的资源
                services.AddSingleton<IBackRunObserverService, BackRunObserverService>();
            };
            return workerServerOption;
        }
    }
}
