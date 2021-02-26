using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 包含容器和自定义数据的后台任务
    /// </summary>
    public abstract class BackRun : IBackRun
    {
        /// <summary>
        /// Host注册的服务，跟asp.net一样使用，只是Scope要自己创建管理
        /// </summary>
        protected IServiceProvider ServiceProvider => WorkerServer.Instance.ServiceProvider;
        /// <summary>
        /// 自定义的数据，在Worker的Context中保存，不同Worker实例的data不同
        /// </summary>
        public IDictionary<string, object> Data;
        /// <summary>
        /// 定义长时间任务时，自己用stoppingToken控制任务尽快结束
        /// </summary>
        /// <param name="stoppingToken">进程结束信号</param>
        /// <returns></returns>
        public abstract Task Run(CancellationToken stoppingToken);
        /// <summary>
        /// 获取host注入的Service,找不到会异常，这里不能获取Scope的Service，必须从<see cref="CreateScope"/>里获取
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetRequiredService<TService>()
        {
            return ServiceProvider.GetService<TService>();
        }
        /// <summary>
        /// 获取host注入的Service,可能返回null，这里不能获取Scope的Service，必须从<see cref="CreateScope"/>里获取
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }
        /// <summary>
        /// 创建一个Scope来自己管理Scoped服务的生存周期，单例和瞬时的服务不需要用这个
        /// </summary>
        /// <returns></returns>
        protected IServiceScope CreateScope()
        {
            return ServiceProvider.CreateScope();
        }
    }
}
