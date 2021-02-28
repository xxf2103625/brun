using System.Threading.Tasks;

namespace Brun
{
    public interface ITimeWorker
    {
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