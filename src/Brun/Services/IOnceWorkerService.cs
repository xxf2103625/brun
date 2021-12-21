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
    public interface IOnceWorkerService
    {
        IOnceWorker GetWorker(string key);
        //Task<BrunResultState> AddOnceBrun(WorkerConfigModel model);
        BrunResultState AddOnceBrun(IOnceWorker onceWorker, Type brunType, OnceBackRunOption option);
        IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns();
        IEnumerable<ValueLabel> GetOnceWorkersInfo();
    }
}