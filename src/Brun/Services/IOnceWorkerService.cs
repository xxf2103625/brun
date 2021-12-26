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
    /// 管理OnceWorker
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
        Task<OnceBackRun> AddOnceBrun(string onceWorkerId, Type brunType, OnceBackRunOption option);
        /// <summary>
        /// 添加OnceBrun
        /// </summary>
        /// <param name="onceWorker"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<OnceBackRun> AddOnceBrun(IOnceWorker onceWorker, Type brunType, OnceBackRunOption option);
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