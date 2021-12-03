using Brun.Models;
using Brun.Workers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    public interface IOnceWorkerService
    {
        Task<BrunResultState> AddOnceBrun(WorkerConfigModel model);
        Task<IEnumerable<WorkerInfo>> GetOnceBruns();
    }
}