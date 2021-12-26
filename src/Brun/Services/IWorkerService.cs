using Brun.Enums;
using Brun.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    public interface IWorkerService
    {
        /// <summary>
        /// 添加worker
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workerType"></param>
        /// <returns></returns>
        Task<IWorker> AddWorker(WorkerConfig model, Type workerType);
        /// <summary>
        /// 添加worker并运行
        /// </summary>
        /// <param name="model"></param>
        /// <param name="workerType"></param>
        /// <returns></returns>
        Task<IWorker> AddWorkerAndStart(WorkerConfig model, Type workerType);
        /// <summary>
        /// 获取Worker
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IWorker> GetWorkerByKey(string key);
        /// <summary>
        /// 获取OnceWorker
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IOnceWorker> GetOnceWorker(string key);
        Task<IQueueWorker> GetQueueWorker(string key);
        Task<IEnumerable<IWorker>> GetWorkerByName(string name);
        Task<(IEnumerable<WorkerInfo>, int)> GetWorkerInfos(int current, int pageSize);
        Task<IEnumerable<IWorker>> GetAllWorkers();
        Task<IEnumerable<IWorker>> GetAllOnceWorkers();
        void Start(string key);
        void StartAll();
        void StartByName(string name);
        void Stop(string key);
        void StopAll();
        void StopByName(string name);
    }
}