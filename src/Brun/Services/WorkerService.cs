using Brun.Enums;
using Brun.Exceptions;
using Brun.Models;
using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// Worker管理，内存中对象操作
    /// </summary>
    public class WorkerService : IWorkerService
    {
        protected WorkerServer workerServer;
        public WorkerService(WorkerServer workerServer)
        {
            this.workerServer = workerServer;
        }
        public virtual Task<IWorker> AddWorker(WorkerConfig model, Type workerType)
        {
            if (workerServer.Worders.ContainsKey(model.Key))
            {
                throw new BrunException(BrunErrorCode.AllreadyKey, $"worker key '{model.Key}' duplicate");
            }
            IWorker worker = (IWorker)Commons.BrunTool.CreateInstance(workerType, model);
            workerServer.Worders.Add(worker.Key, worker);
            return Task.FromResult(worker);
        }
        public virtual async Task<IWorker> AddWorkerAndStart(WorkerConfig model, Type workerType)
        {
            var worker = await AddWorker(model, workerType);
            this.Start(model.Key);
            return worker;
        }
        public virtual Task<IWorker> GetWorkerByKey(string key)
        {
            if (workerServer.Worders.ContainsKey(key))
            {
                return Task.FromResult(workerServer.Worders[key]);
            }
            else
            {
                throw new BrunException(BrunErrorCode.NotFoundKey, $"can not find worker by key:'{key}'");
            }
        }
        public virtual Task<IEnumerable<IWorker>> GetWorkerByName(string name)
        {
            return Task.FromResult(workerServer.Worders.Values.Where(x => x.Name == name));
        }
        public virtual Task<(IEnumerable<WorkerInfo>, int)> GetWorkerInfos(int current, int pageSize)
        {
            var list = workerServer.Worders.Values.OrderBy(m => m.Name).Skip(pageSize * (current - 1)).Take(pageSize).Select(m => new WorkerInfo()
            {
                Key = m.Key,
                Name = m.Name,
                TypeName = m.GetType().Name,
                State = m.State
            });
            return Task.FromResult((list, workerServer.Worders.Count));
        }
        public virtual void Start(string key)
        {
            if (workerServer.Worders.TryGetValue(key, out IWorker worker))
            {
                ((AbstractWorker)worker).ProtectStart();
            }
            else
            {
                throw new BrunException(BrunErrorCode.NotFoundKey, $"worker start error,can not find key:'{key}'");
            }

        }
        public virtual void StartAll()
        {
            for (int i = 0; i < workerServer.Worders.Values.Count; i++)
            {
                ((AbstractWorker)workerServer.Worders.Values.ElementAt(i)).ProtectStart();
            }
        }
        public virtual void Stop(string key)
        {
            if (workerServer.Worders.TryGetValue(key, out IWorker worker))
            {
                ((AbstractWorker)worker).ProtectStop();
            }
            else
            {
                throw new BrunException(BrunErrorCode.NotFoundKey, $"worker start error,can not find key:'{key}'");
            }
        }

        public virtual void StopAll()
        {
            for (int i = 0; i < workerServer.Worders.Values.Count; i++)
            {
                ((AbstractWorker)workerServer.Worders.Values.ElementAt(i)).ProtectStop();
            }
        }
        public virtual void StartByName(string name)
        {
            throw new NotImplementedException();
        }
        public virtual void StopByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
