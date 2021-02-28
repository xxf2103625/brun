using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    public abstract class BackRunServicePrivoder
    {
        /// <summary>
        /// Host注册的服务，跟asp.net一样使用，只是Scope要自己创建管理
        /// </summary>
        protected IServiceProvider ServiceProvider => WorkerServer.Instance.ServiceProvider;
        

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
