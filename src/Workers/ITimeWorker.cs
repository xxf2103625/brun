using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 时间周期执行的Worker
    /// </summary>
    public interface ITimeWorker
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        Task Start();
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
    }
}