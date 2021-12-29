﻿using Brun.Enums;
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
        public virtual Task<IWorker> AddWorker(WorkerConfig config, Type workerType, bool autoStart = true)
        {
            if (workerServer.Worders.ContainsKey(config.Key))
            {
                throw new BrunException(BrunErrorCode.AllreadyKey, $"worker key '{config.Key}' duplicate");
            }
            IWorker worker = (IWorker)Commons.BrunTool.CreateInstance(workerType, config);
            workerServer.Worders.Add(worker.Key, worker);
            if (autoStart)
                this.Start(worker.Key);
            return Task.FromResult(worker);
        }
        public virtual async Task<TWorker> AddWorker<TWorker>(WorkerConfig config, bool autoStart = true) where TWorker : AbstractWorker
        {
            return (TWorker)(await AddWorker(config, typeof(TWorker), autoStart));
        }
        public virtual async Task<IOnceWorker> AddOnceWorker(WorkerConfig workerConfig, bool autoStart = true)
        {
            return await AddWorker<OnceWorker>(workerConfig, autoStart);
        }
        public virtual async Task<ITimeWorker> AddTimeWorker(WorkerConfig workerConfig, bool autoStart = true)
        {
            return await AddWorker<TimeWorker>(workerConfig, autoStart);
        }
        public virtual async Task<IQueueWorker> AddQueueWorker(WorkerConfig config, bool autoStart = true)
        {
            return await AddWorker<QueueWorker>(config, autoStart);
        }
        public virtual async Task<IPlanWorker> AddPlanWorker(WorkerConfig config, bool autoStart = true)
        {
            return await AddWorker<PlanWorker>(config, autoStart);
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
        public virtual async Task<IOnceWorker> GetOnceWorkerByKey(string key)
        {
            var worker = await GetWorkerByKey(key);
            if (worker.GetType() != typeof(OnceWorker))
                throw new BrunException(BrunErrorCode.NotFoundKey, $"the worker by key:'{key}' is not OnceWorker");
            return (IOnceWorker)worker;
        }
        public virtual async Task<IQueueWorker> GetQueueWorker(string key)
        {
            var worker = await GetWorkerByKey(key);
            if (worker.GetType() != typeof(QueueWorker))
                throw new BrunException(BrunErrorCode.NotFoundKey, $"the worker by key:'{key}' is not QueueWorker");
            return (IQueueWorker)worker;
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
        public virtual Task<IEnumerable<IWorker>> GetAllWorkers()
        {
            return Task.FromResult(workerServer.Worders.Values.AsEnumerable());
        }
        public virtual Task<IEnumerable<OnceWorker>> GetAllOnceWorkers()
        {
            return Task.FromResult(workerServer.Worders.Values.Where(m => m.GetType() == typeof(OnceWorker)).Cast<OnceWorker>().AsEnumerable());
        }
        public virtual Task<IEnumerable<IWorker>> GetAllTimeWorkers()
        {
            return Task.FromResult(workerServer.Worders.Values.Where(m => m.GetType() == typeof(TimeWorker)).AsEnumerable());
        }
        public virtual Task<IEnumerable<IWorker>> GetQueueTimeWorkers()
        {
            return Task.FromResult(workerServer.Worders.Values.Where(m => m.GetType() == typeof(QueueWorker)).AsEnumerable());
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
