using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 消息队列Worker
    /// </summary>
    public interface IQueueWorker : IWorker
    {
        /// <summary>
        /// 添加QueueBackRun
        /// </summary>
        /// <param name="queueBackRunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IQueueWorker> AddBrun(Type queueBackRunType, QueueBackRunOption option);
        /// <summary>
        /// 添加QueueBackRun
        /// </summary>
        /// <typeparam name="TQueueBackRun"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IQueueWorker> AddBrun<TQueueBackRun>(QueueBackRunOption option) where TQueueBackRun : QueueBackRun;
        /// <summary>
        /// 给Id为brunId的QueueBackRun发送消息
        /// </summary>
        /// <param name="brunId"></param>
        /// <param name="message"></param>
        void Enqueue(string brunId, string message);
        /// <summary>
        /// 给默认的QueueBackRun发送消息
        /// </summary>
        /// <param name="message"></param>
        void Enqueue(string message);
        /// <summary>
        /// 给TQueueBackRun发送消息
        /// </summary>
        /// <typeparam name="TQueueBackRun"></typeparam>
        /// <param name="message"></param>
        void Enqueue<TQueueBackRun>(string message);
        /// <summary>
        /// 给queueBackRunType发送消息
        /// </summary>
        /// <param name="queueBackRunType"></param>
        /// <param name="message"></param>
        void Enqueue(Type queueBackRunType, string message);
        /// <summary>
        /// 给指定类型的QueueBackRun发送消息
        /// </summary>
        /// <param name="queueBackRunTypeFullName">含命名空间的类型全名</param>
        /// <param name="message"></param>
        void EnqueueByType(string queueBackRunTypeFullName, string message);
    }
}
