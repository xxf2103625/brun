using Brun.Options;
using System;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 时间周期执行的Worker
    /// </summary>
    public interface ITimeWorker
    {
        /// <summary>
        /// 添加TimeBackRun
        /// </summary>
        /// <param name="timeBackRunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<ITimeWorker> AddBrun(Type timeBackRunType, TimeBackRunOption option);
        /// <summary>
        /// 添加TimeBackRun
        /// </summary>
        /// <typeparam name="TTimeBackRun"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<ITimeWorker> AddBrun<TTimeBackRun>(TimeBackRunOption option) where TTimeBackRun : TimeBackRun;
    }
}