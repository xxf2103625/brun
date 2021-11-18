using Brun.Workers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 工作中心，每个实例会常驻进程
    /// </summary>
    public interface IWorker : IDisposable
    {
        /// <summary>
        /// Worker唯一Id
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Worker名称，配置时指定，不指定为类型名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Worker上下文
        /// </summary>
        WorkerContext Context { get; }
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        void Start();
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        void Stop();



        ///// <summary>
        ///// TaskFactory
        ///// </summary>
        ////TaskFactory TaskFactory { get; }
        ///// <summary>
        ///// 运行中的Task
        ///// </summary>
        ////BlockingCollection<Task> RunningTasks { get; }
        ///// <summary>
        ///// 转换到IOnceWorker
        ///// </summary>
        ///// <returns></returns>
        ////IOnceWorker AsOnceWorker();
        ///// <summary>
        ///// 类型转换
        ///// </summary>
        ///// <returns></returns>
        ////IQueueWorker AsQueueWorker();
        ///// <summary>
        ///// 类型转换
        ///// </summary>
        ///// <returns></returns>
        ////ITimeWorker AsTimeWOrker();
        ///// <summary>
        ///// 类型转换
        ///// </summary>
        ///// <returns></returns>
        ////IPlanTimeWorker AsPlanTimeWorker();
        ////IEnumerable<Type> BrunTypes { get; }
    }
}
