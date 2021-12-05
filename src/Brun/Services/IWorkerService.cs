using Brun.Enums;
using Brun.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    public interface IWorkerService
    {
        Task<BrunResultState> AddWorker(WorkerConfigModel model, WorkerType workerType);
        IEnumerable<WorkerInfo> GetWorkerInfos();
        void Start(string key);
        void StartAll();
        void StartByName(string name);
        void Stop(string key);
        void StopAll();
        void StopByName(string name);
    }
}