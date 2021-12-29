using System;
using System.Collections.Generic;

namespace Brun.Services
{
    /// <summary>
    /// 查询程序集内用户自己实现的BackRun
    /// </summary>
    public interface IBackRunFilterService
    {
        /// <summary>
        /// 获取所有用户自定义的BackRun
        /// </summary>
        /// <returns></returns>
        List<Type> GetBackRunTypes();
        /// <summary>
        /// 获取所有可用的OnceBackRun
        /// </summary>
        /// <returns></returns>
        List<Type> GetOnceBackRunTypes();
        /// <summary>
        /// 获取所有可用的PlanBackRun
        /// </summary>
        /// <returns></returns>
        List<Type> GetPlanBackRunTypes();
        /// <summary>
        /// 获取所有可用的QueueBackRun
        /// </summary>
        /// <returns></returns>
        List<Type> GetQueueBackRunTypes();
        /// <summary>
        /// 获取所有可用的TimeBackRun
        /// </summary>
        /// <returns></returns>
        List<Type> GetTimeBackRunTypes();
    }
}