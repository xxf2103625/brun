using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brun
{
    /// <summary>
    /// 注册入口
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册 配置 并启用Brun组件
        /// </summary>
        /// <param name="services"></param>
        /// <param name="workerServerOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddBrunService(this IServiceCollection services, Action<WorkerServerOption> workerServerOptions = null)
        {
            var worker = WorkerServer.Instance;
            WorkerServerOption option = new WorkerServerOption();
            if (workerServerOptions == null)
            {
                //默认内存运行,持久化需要安装扩展库
                option.UseInMemory();
            }
            else
            {
                workerServerOptions.Invoke(option);
                //没有配置持久化时使用内存模式
                if (option.StoreType == WorkerStoreType.None)
                {
                    option.UseInMemory();
                }
            }
            if (option.ServicesConfigure != null)
            {
                //扩展库的服务注册/替换
                option.ServicesConfigure(services);
            }
            worker.Option = option;

            //TODO 初始化加载未包含的程序集
            Brun.Commons.BrunTool.LoadFile("BrunTestHelper.dll");

            services.AddSingleton<WorkerServer>(m => worker);
            services.AddSingleton<IBackRunFilterService, BackRunFilterService>();
            services.AddHostedService<BrunBackgroundService>();
            return services;
        }
    }
}
