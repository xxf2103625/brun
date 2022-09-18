using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    /// <summary>
    /// 所有后台任务的接口
    /// </summary>
    public interface IBackRun
    {
        /// <summary>
        /// BackRun的Key
        /// </summary>
        string Id { get; }
        /// <summary>
        /// BackRun的Name
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 启动总次数
        /// </summary>
        long StartTimes { get; }
        /// <summary>
        /// 异常次数
        /// </summary>
        long ErrorTimes { get; }
        long EndTimes { get; }
        /// <summary>
        /// 最后一次异常Id
        /// </summary>
        string LastErrorId { get; }
        /// <summary>
        /// 单个Worker实例中的共享数据
        /// </summary>
        ConcurrentDictionary<string, string> Data { get; }
        /// <summary>
        /// Worker实例上下文
        /// </summary>
        WorkerContext WorkerContext { get; }
        IServiceProvider ServiceProvider { get; }
        TService GetRequiredService<TService>();
        TService GetService<TService>();
        object GetService(Type serviceType);
        IServiceScope CreateScope();
        AsyncServiceScope CreateAsyncScope(Type serviceType);
    }
}
