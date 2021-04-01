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
        public static IServiceCollection AddBrunService(this IServiceCollection services)
        {
            services.AddSingleton<IWorkerServer, WorkerServer>(m => WorkerServer.Instance);
            services.AddSingleton<BrunMonitor>();
            services.AddHostedService<BrunBackgroundService>();
            return services;
        }
        /// <summary>
        /// 以服务形式启动Brun.在ConfigureServices中调用.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">和在ConfigureServices中注入服务没有任何区别，可能你需要在代码位置上明确这些服务只给Brun使用</param>
        /// <returns></returns>
        public static IServiceCollection AddBrunService(this IServiceCollection services, Action<IServiceCollection> configure)
        {
            AddBrunService(services);
            configure.Invoke(services);
            return services;
        }
    }
}
