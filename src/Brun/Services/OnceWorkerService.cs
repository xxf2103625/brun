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
        public Task<BrunResultState> AddOnceBrun(WorkerConfigModel model)
        {
            if (model.Key == null)
            {
                model.Key = Guid.NewGuid().ToString();
            }
            if (model.Name == null)
            {
                model.Name = nameof(OnceWorker);
            }
            if (_baseService.ExistWorkerKey(model.Key))
            {
                return Task.FromResult(BrunResultState.IdBeUsed);
            }
            _baseService.AddWorker(model);
            return Task.FromResult(BrunResultState.Success);
        }
        public Task<IEnumerable<WorkerInfo>> GetOnceBruns()
        {
            var workers = _baseService.GetWorkers().ToList().Select(m => new WorkerInfo()
            {
                Key = m.Key,
                Name = m.Name
            });

            return Task.FromResult(workers);
        }
    }
}
