using System;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// 队列任务相关服务
    /// </summary>
    public interface IQueueBrunService
    {
        /// <summary>
        /// 添加QueueBackRun
        /// </summary>
        /// <param name="queueWorker"></param>
        /// <param name="queueBackRunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        IQueueWorker AddQueueBrun(IQueueWorker queueWorker, Type queueBackRunType, QueueBackRunOption option);
        /// <summary>
        /// 添加QueueBackRun
        /// </summary>
        /// <typeparam name="TQueueBackRun"></typeparam>
        /// <param name="queueWorker"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        IQueueWorker AddQueueBrun<TQueueBackRun>(IQueueWorker queueWorker, QueueBackRunOption option) where TQueueBackRun : QueueBackRun;
    }
}