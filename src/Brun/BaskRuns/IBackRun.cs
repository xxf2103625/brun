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
        /// TODO 是否移入上下文
        /// 单个Worker实例中的共享数据
        /// </summary>
        ConcurrentDictionary<string, string> Data { get; }
        WorkerContext WorkerContext { get; }
    }
}
