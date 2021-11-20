using Brun.Models;
using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// 内存模式管理OnceWorker
    /// </summary>
    public class OnceWorkerService : IOnceWorkerService
    {
        private WorkerServer _workerServer;
        IBaseWorkerService<OnceWorker> _baseService;
        public OnceWorkerService(WorkerServer workerServer, IBaseWorkerService<OnceWorker> baseWorkerService)
        {
            _workerServer = workerServer;
            _baseService = baseWorkerService;
        }
        /// <summary>
        /// 添加OnceWorker
        /// </summary>
        /// <param name="model"></param>
        public Task<BrunResultState> AddOnceWorker(WorkerConfigModel model)
        {
            if (model.Key == null)
            {
                model.Key = Guid.NewGuid().ToString();
            }
            if (model.Name == null)
            {
                model.Name = nameof(OnceWorker);
            }
            if (_workerServer.Worders.Any(m => m.Key == model.Key))
            {
                return Task.FromResult(BrunResultState.IdBeUsed);
            }
            _baseService.AddWorker(model);
            return Task.FromResult(BrunResultState.Success);
        }
        public Task<IEnumerable<OnceWorker>> GetOnceWorkers()
        {
            return Task.FromResult(_baseService.GetWorkers());
        }
    }
}
