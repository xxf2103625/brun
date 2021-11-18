using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brun
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 以服务形式启动Brun.在ConfigureServices中调用.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        [Obsolete("重构")]
        public static IServiceCollection AddBrunService(this IServiceCollection services)
        {
            //TODO 迁移到扩展库
            if (WorkerServer.Instance.ServerConfig.UseSystemBrun)
            {
                //WorkerBuilder.CreatePlanTime<SystemBackRun>("0 * * * * ")//每分钟
                //       .SetKey(SystemBackRun.Worker_KEY)
                //       .SetName("Brun系统监控")
                //       .Build()
                //       ;
            }
            services.AddSingleton<IWorkerServer, WorkerServer>(m => WorkerServer.Instance);
            //services.AddSingleton<BrunService>();
            services.AddHostedService<BrunBackgroundService>();
            return services;
        }
        /// <summary>
        /// 以服务形式启动Brun.在ConfigureServices中调用.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="workerServer"></param>
        /// <returns></returns>
        public static IServiceCollection AddBrunService(this IServiceCollection services, Action<WorkerServer> workerServer)
        {
            //TODO 迁移到扩展库
            if (WorkerServer.Instance.ServerConfig.UseSystemBrun)
            {
                //WorkerBuilder.CreatePlanTime<SystemBackRun>("0 * * * * ")//每分钟
                //       .SetKey(SystemBackRun.Worker_KEY)
                //       .SetName("Brun系统监控")
                //       .Build()
                //       ;
            }
            WorkerServer.Instance.Configure = workerServer;
            services.AddSingleton<IWorkerServer, WorkerServer>(m => WorkerServer.Instance);
            //services.AddSingleton<BrunService>();
            services.AddHostedService<BrunBackgroundService>();
            return services;
        }
        /// <summary>
        /// 以服务形式启动Brun.在ConfigureServices中调用.
        /// </summary>
        /// <param name="serviceDescriptors"></param>
        /// <param name="workerServer"></param>
        /// <param name="services">和在ConfigureServices中注入服务没有任何区别，可能你需要在代码位置上明确这些服务只给Brun使用</param>
        /// <returns></returns>
        public static IServiceCollection AddBrunService(this IServiceCollection serviceDescriptors, Action<WorkerServer> workerServer, Action<IServiceCollection> services)
        {
            serviceDescriptors.AddBrunService(workerServer);
            services.Invoke(serviceDescriptors);
            return serviceDescriptors;
        }
    }
}
