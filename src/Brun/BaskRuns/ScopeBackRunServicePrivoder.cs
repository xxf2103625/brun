﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    /// <summary>
    /// //TODO 是否需要封装ServiceProvider操作
    /// </summary>
    public abstract class IocBackRunServicePrivoder
    {
        /// <summary>
        /// Host注册的服务，跟asp.net一样使用，只是Scope注册的要自己创建Scope
        /// </summary>
        protected IServiceProvider ServiceProvider => WorkerServer.Instance.ServiceProvider;
        /// <summary>
        /// 获取Ioc注入的Service,找不到会异常，这里不能获取Scope的Service，必须从<see cref="NewScope"/>里获取
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetRequiredService<TService>()
        {
            return ServiceProvider.GetRequiredService<TService>();
        }
        /// <summary>
        /// 获取Ioc注入的Service,可能返回null，这里不能获取Scope的Service，必须从<see cref="NewScope"/>里获取
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        protected TService GetService<TService>()
        {
            return ServiceProvider.GetService<TService>();
        }
        /// <summary>
        /// 创建一个Scope来自己管理Scoped服务的生存周期，单例和瞬时的服务不需要用这个
        /// </summary>
        /// <returns></returns>
        protected IServiceScope NewScope()
        {
            return ServiceProvider.CreateScope();
        }
    }
}
