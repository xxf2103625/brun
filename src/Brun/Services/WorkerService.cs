﻿using Brun.Enums;
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
    /// Worker管理，内存中
    /// </summary>
    public class WorkerService
    {
        private WorkerServer workerServer;
        public WorkerService(WorkerServer workerServer)
        {
            this.workerServer = workerServer;
        }
        public Task<BrunResultState> AddWorker(WorkerConfigModel model, WorkerType workerType)
        {
            Type type = GetWorkerType(workerType);
            if (model.Key == null)
            {
                model.Key = Guid.NewGuid().ToString();
            }
            if (model.Name == null)
            {
                model.Name = type.Name;
            }
            if (workerServer.Worders.ContainsKey(model.Key))
            {
                return Task.FromResult(BrunResultState.IdBeUsed);
            }
            var worker = workerServer.CreateWorker(type, new WorkerConfig(model.Key, model.Name));
            workerServer.Worders.Add(worker.Key, worker);
            return Task.FromResult(BrunResultState.Success);
        }
        public Task<List<WorkerInfo>> GetWorkerInfos()
        {
            var list = workerServer.Worders.Values.Select(m => new WorkerInfo()
            {
                Key = m.Key,
                Name = m.Name,
                TypeName = m.GetType().Name
            }).ToList();
            return Task.FromResult(list);
        }
        public void Start(string key)
        {
            throw new NotImplementedException();
        }
        public void StartByName(string name)
        {
            throw new NotImplementedException();
        }
        public void StartAll()
        {
            throw new NotImplementedException();
        }
        public void Stop(string key)
        {
            throw new NotImplementedException();
        }
        public void StopByName(string name)
        {
            throw new NotImplementedException();
        }
        public void StopAll()
        {
            throw new NotImplementedException();
        }
        private Type GetWorkerType(WorkerType workerType)
        {
            switch (workerType)
            {
                case WorkerType.OnceWorker:
                    return typeof(OnceWorker);
                case WorkerType.TimeWorker:
                    return typeof(TimeWorker);
                case WorkerType.QueueWorker:
                    return typeof(QueueWorker);
                case WorkerType.PlanWorker:
                    return typeof(PlanWorker);
                default:
                    throw new Exceptions.BrunException(Exceptions.BrunErrorCode.TypeError, "worker type error");
            }
        }
    }
}
