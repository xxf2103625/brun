using Brun.Models;
using Brun.Workers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    public interface IOnceWorkerService
    {
        Task<BrunResultState> AddOnceWorker(WorkerConfigModel model);
        Task<IEnumerable<OnceWorker>> GetOnceWorkers();
    }
}