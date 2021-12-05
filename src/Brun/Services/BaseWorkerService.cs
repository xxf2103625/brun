using Brun.BaskRuns;
using Brun.Exceptions;
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
    /// 封装Brun实例对象常用操作
    /// </summary>
    public class BaseWorkerService<TWorker> : IBaseWorkerService<TWorker> where TWorker : AbstractWorker
    {
        private WorkerServer _workerServer;
        public BaseWorkerService(WorkerServer workerServer)
        {
            _workerServer = workerServer;
        }
        public void AddWorker(WorkerConfigModel model)
        {
            var worker = _workerServer.CreateWorker<TWorker>(new WorkerConfig(model.Key, model.Name));
            if (_workerServer.Worders.Any(m => m.Key == model.Key))
            {
                throw new BrunException(BrunErrorCode.AllreadyKey, "add Worker key existed");
            }
            _workerServer.Worders.Add(worker.Key, worker);
            //return BrunResultState.Success;
        }
        public IEnumerable<TWorker> GetWorkers()
        {
            return _workerServer.Worders.Where(m => m.Value.GetType() == typeof(TWorker)).Select(m => m.Value).Cast<TWorker>();
        }
        public TWorker GetWorkerByKey(string key)
        {
            return _workerServer.GetWorker<TWorker>(key);
        }
        public IEnumerable<KeyValuePair<string, IBackRun>> GetBackRuns()
        {
            foreach (var item in GetWorkers())
            {
                foreach (var brun in item.BackRuns)
                {
                    yield return brun;
                }
            }
        }
        /// <summary>
        /// 是否已有key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ExistWorkerKey(string key)
        {
            return _workerServer.Worders.Any(m => m.Key == key);
        }
        ///// <summary>
        ///// 添加OnceWorker
        ///// </summary>
        ///// <param name="model"></param>
        //public void AddOnceWorker(WorkerConfigModel model)
        //{
        //    _workerServer.CreateWorker<OnceWorker>(new WorkerConfig(model.Key, model.Name));
        //}
        ///// <summary>
        ///// 添加TimeWorker
        ///// </summary>
        ///// <param name="model"></param>
        //public void AddTimeWorker(WorkerConfigModel model)
        //{
        //    _workerServer.CreateWorker<TimeWorker>(new WorkerConfig(model.Key, model.Name));
        //}
        ///// <summary>
        ///// 添加QueueWorker
        ///// </summary>
        ///// <param name="model"></param>
        //public void AddQueueWorker(WorkerConfigModel model)
        //{
        //    _workerServer.CreateWorker<QueueWorker>(new WorkerConfig(model.Key, model.Name));
        //}
        ///// <summary>
        ///// 添加PlanWorker
        ///// </summary>
        ///// <param name="model"></param>
        //public void AddPlanWorker(WorkerConfigModel model)
        //{
        //    _workerServer.CreateWorker<PlanWorker>(new WorkerConfig(model.Key, model.Name));
        //}
    }
}
