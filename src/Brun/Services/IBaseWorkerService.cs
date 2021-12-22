using Brun.BaskRuns;
using Brun.Models;
using Brun.Workers;
using System.Collections.Generic;

namespace Brun.Services
{
    public interface IBaseWorkerService<TWorker> where TWorker : AbstractWorker
    {
        IEnumerable<KeyValuePair<string, IBackRun>> GetBackRuns();
    }
}