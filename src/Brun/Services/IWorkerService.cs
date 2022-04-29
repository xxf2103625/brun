using Brun.Enums;
using Brun.Models;
using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// 所有Worker的操作接口
    /// </summary>
    public interface IWorkerService
    {
        /// <summary>
        /// 添加worker
        /// </summary>
        /// <param name="config"></param>
        /// <param name="workerType"></param>
        /// <param name="autoStart"></param>
        /// <param name="addRunDetailObserver"></param>
        /// <returns></returns>
        IWorker AddWorker(WorkerConfig config, Type workerType, bool autoStart = true, bool addRunDetailObserver = true);
        /// <summary>
        /// 添加worker
        /// </summary>
        /// <typeparam name="TWorker"></typeparam>
        /// <param name="config"></param>
        /// <param name="autoStart"></param>
        /// <returns></returns>
        TWorker AddWorker<TWorker>(WorkerConfig config, bool autoStart = true) where TWorker : AbstractWorker;
        /// <summary>
        /// 添加OnceWorker
        /// </summary>
        /// <param name="workerConfig"></param>
        /// <param name="autoStart">默认立即Start</param>
        /// <returns></returns>
        IOnceWorker AddOnceWorker(WorkerConfig workerConfig, bool autoStart = true);
        /// <summary>
        /// 添加TimeWorker
        /// </summary>
        /// <param name="workerConfig"></param>
        /// <param name="autoStart">默认立即Start</param>
        /// <returns></returns>
        ITimeWorker AddTimeWorker(WorkerConfig workerConfig, bool autoStart = true);
        /// <summary>
        /// 添加QueueWorker
        /// </summary>
        /// <param name="workerConfig"></param>
        /// <param name="autoStart"></param>
        /// <returns></returns>
        IQueueWorker AddQueueWorker(WorkerConfig workerConfig, bool autoStart = true);
        /// <summary>
        /// 添加PlanWorker
        /// </summary>
        /// <param name="workerConfig"></param>
        /// <param name="autoStart"></param>
        /// <returns></returns>
        IPlanWorker AddPlanWorker(WorkerConfig workerConfig, bool autoStart = true);
        /// <summary>
        /// 获取Worker
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IWorker GetWorkerByKey(string key);
        /// <summary>
        /// 获取OnceWorker
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IOnceWorker GetOnceWorkerByKey(string key);
        IQueueWorker GetQueueWorker(string key);
        IEnumerable<IWorker> GetWorkerByName(string name);
        (IEnumerable<WorkerInfo>, int) GetWorkerInfos(int current, int pageSize);
        IEnumerable<IWorker> GetAllWorkers();
        IEnumerable<OnceWorker> GetAllOnceWorkers();
        IEnumerable<TimeWorker> GetAllTimeWorkers();
        void Start(string key);
        void StartAll();
        void StartByName(string name);
        void Stop(string key);
        void StopAll();
        void StopByName(string name);
    }
}