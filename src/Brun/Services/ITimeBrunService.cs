using Brun.BaskRuns;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    public interface ITimeBrunService
    {
        ITimeWorker AddTimeBrun(ITimeWorker timeWorker, Type brunType, TimeBackRunOption option);
        ITimeWorker AddTimeBrun<TTimeBackRun>(ITimeWorker timeWorker, TimeBackRunOption option) where TTimeBackRun : TimeBackRun;
        IEnumerable<KeyValuePair<string, IBackRun>> GetTimeBruns();
    }
}