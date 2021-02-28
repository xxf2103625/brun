using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 工作中心，每个实例会常驻进程，除非Destroy
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
        /// Worker分组标签
        /// </summary>
        string Tag { get; }
        /// <summary>
        /// Worker上下文
        /// </summary>
        WorkerContext Context { get; }
        /// <summary>
        /// 直接运行不用等待,适合web中使用
        /// </summary>
        void RunDontWait();
        /// <summary>
        /// 运行，web中不要await
        /// </summary>
        /// <returns></returns>
        Task Run();
        /// <summary>
        /// 暂停
        /// </summary>
        /// <returns></returns>
        [Obsolete("暂不支持", true)]
        Task Pause();
        /// <summary>
        /// 恢复
        /// </summary>
        /// <returns></returns>
        [Obsolete("暂不支持", true)]
        Task Resume();
        /// <summary>
        /// 销毁
        /// </summary>
        /// <returns></returns>
        [Obsolete("暂不支持", true)]
        Task Destroy();
        /// <summary>
        /// 自定义数据
        /// </summary>
        /// <returns></returns>
        ConcurrentDictionary<string, string> GetData();

        string GetData(string key);
        [Obsolete("暂不支持", true)]
        T GetData<T>(string key);

    }
}
