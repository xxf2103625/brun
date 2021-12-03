using Brun.Models;
using Brun.Workers;
using System.Collections.Generic;

namespace Brun.Services
{
    public interface IBaseWorkerService<TWorker> where TWorker : AbstractWorker
    {
        /// <summary>
        /// 添加worker
        /// </summary>
        /// <param name="model"></param>
        void AddWorker(WorkerConfigModel model);
        /// <summary>
        /// 获取worker
        /// </summary>
        /// <returns></returns>
        IEnumerable<TWorker> GetWorkers();
        /// <summary>
        /// 是否已有key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ExistWorkerKey(string key);
    }
}