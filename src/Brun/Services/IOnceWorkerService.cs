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
        //Task<BrunResultState> AddOnceBrun(WorkerConfigModel model);
        Task<OnceBackRun> AddOnceBrun(IOnceWorker onceWorker, Type brunType, OnceBackRunOption option);
        Task<IEnumerable<KeyValuePair<string, IBackRun>>> GetOnceBruns();
        IEnumerable<ValueLabel> GetAllUserOnceBruns();
    }
}