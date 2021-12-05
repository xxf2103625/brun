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
        //Task<BrunResultState> AddOnceBrun(WorkerConfigModel model);
        BrunResultState AddOnceBrun(OnceWorker onceWorker, Type brunType, BackRunOption option);
        IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns();
        IEnumerable<ValueLabel> GetOnceWorkersInfo();
    }
}