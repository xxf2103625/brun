using Brun.BaskRuns;
using Brun.Models;
using Brun.Options;
using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// 管理OnceBackRun的接口
    /// </summary>
    public interface IOnceBrunService
    {
        /// <summary>
        /// 添加OnceBrun
        /// </summary>
        /// <param name="onceWorkerId"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IOnceWorker> AddOnceBrun(string onceWorkerId, Type brunType, OnceBackRunOption option);
        /// <summary>
        /// 添加OnceBrun 泛型
        /// </summary>
        /// <typeparam name="TOnceBackRun"></typeparam>
        /// <param name="onceWorkerId"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IOnceWorker> AddOnceBrun<TOnceBackRun>(string onceWorkerId, OnceBackRunOption option) where TOnceBackRun : OnceBackRun;
        /// <summary>
        /// 添加OnceBrun
        /// </summary>
        /// <param name="onceWorker"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IOnceWorker> AddOnceBrun(IOnceWorker onceWorker, Type brunType, OnceBackRunOption option);
        Task<IOnceWorker> AddOnceBrun<TOnceBackRun>(IOnceWorker onceWorker, OnceBackRunOption option) where TOnceBackRun : OnceBackRun;
        /// <summary>
        /// 执行指定Worker中指定Id的OnceBackrun
        /// </summary>
        /// <param name="workerId"></param>
        /// <param name="onceBackRunId"></param>
        /// <returns></returns>
        Task Run(string workerId, string onceBackRunId);
        /// <summary>
        /// 执行指定Id的OnceBackRun（多一步查询worker）
        /// </summary>
        /// <param name="onceBackRunId"></param>
        /// <returns></returns>
        Task Run(string onceBackRunId);
        /// <summary>
        /// 获取所有已配置的OnceBrun
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<KeyValuePair<string, IBackRun>>> GetOnceBruns();
        /// <summary>
        /// 获取所有可用的OnceBrun
        /// </summary>
        /// <returns></returns>
        IEnumerable<ValueLabel> GetAllUserOnceBruns();
    }
}