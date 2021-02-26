using System;
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
        Task Pause();
        /// <summary>
        /// 恢复
        /// </summary>
        /// <returns></returns>
        Task Resume();
        /// <summary>
        /// 销毁
        /// </summary>
        /// <returns></returns>
        Task Destroy();
        string Key { get; }

        string Name { get; }

        string Tag { get; }
        public WorkerContext Context { get; }
        public IDictionary<string, object> GetData();

        public object GetData(string key);

        public T GetData<T>(string key);

    }
}
