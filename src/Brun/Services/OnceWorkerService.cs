﻿using Brun.BaskRuns;
using Brun.Models;
using Brun.Options;
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
        //private readonly WorkerServer workerServer;
        IBaseWorkerService<OnceWorker> baseService;
        IWorkerService workerService;
        public OnceWorkerService(IWorkerService workerService, IBaseWorkerService<OnceWorker> baseWorkerService)
        {
            //this.workerServer = workerServer;
            this.baseService = baseWorkerService;
            this.workerService = workerService;
        }
        public IOnceWorker GetWorker(string key)
        {
            return (IOnceWorker)workerService.GetWorkerByKey(key);
            //return baseService.GetWorkerByKey(key);
        }
        /// <summary>
        /// 添加OnceWorker
        /// </summary>
        /// <param name="onceWorker"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public BrunResultState AddOnceBrun(IOnceWorker onceWorker, Type brunType, OnceBackRunOption option)
        {
            return onceWorker.AddBrun(brunType, option);
        }
        public IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns()
        {
            return this.baseService.GetBackRuns();
        }
        //public IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns(int current, int pageSize)
        //{
        //    return this.baseService.GetBackRuns().Skip((current-1)*pageSize);
        //}
        public IEnumerable<ValueLabel> GetOnceWorkersInfo()
        {
            throw new NotImplementedException();
            //return this.baseService.GetWorkers().Select(m => new ValueLabel(m.Key, m.Name));
        }

    }
}
