using Brun.BaskRuns;
using Brun.Models;
using Brun.Workers;
using System;
using System.Collections.Generic;

namespace Brun.Services
{
    [Obsolete]
    internal interface IBaseWorkerService<TWorker> where TWorker : AbstractWorker
    {
        
        IEnumerable<KeyValuePair<string, IBackRun>> GetBackRuns();
    }
}