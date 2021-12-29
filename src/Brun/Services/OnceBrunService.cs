using Brun.BaskRuns;
using Brun.Exceptions;
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
    /// 内存模式管理OnceBrun
    /// </summary>
    public class OnceBrunService : IOnceBrunService
    {
        IWorkerService workerService;
        IBackRunFilterService backRunFilterService;
        public OnceBrunService(IWorkerService workerService, IBackRunFilterService backRunFilterService)
        {
            this.workerService = workerService;
            this.backRunFilterService = backRunFilterService;
        }
        public async Task<OnceBackRun> AddOnceBrun(string onceWorkerId, Type brunType, OnceBackRunOption option)
        {
            if (brunType == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, $"add once brun error,OnceBackrun Type is null");
            var worker = await workerService.GetOnceWorkerByKey(onceWorkerId);
            if (worker == null)
                throw new BrunException(BrunErrorCode.NotFoundKey, $"add once brun error,can not find OnceWorker by key:'{onceWorkerId}'");
            return worker.AddBrun(brunType, option);
        }
        /// <summary>
        /// 添加OnceBrun
        /// </summary>
        /// <param name="onceWorker"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public Task<OnceBackRun> AddOnceBrun(IOnceWorker onceWorker, Type brunType, OnceBackRunOption option)
        {
            return Task.FromResult(onceWorker.AddBrun(brunType, option));
        }
        public async Task Run(string onceBackRunId)
        {
            IEnumerable<IOnceWorker> workers = await workerService.GetAllOnceWorkers();
            foreach (OnceWorker worker in workers)
            {
                if (worker.BackRuns.ContainsKey(onceBackRunId))
                {
                    worker.Run(onceBackRunId);
                    return;
                }
            }
            throw new BrunException(BrunErrorCode.NotFoundKey, $"can not find online OnceBackRun by id:'{onceBackRunId}'");
        }
        public async Task Run(string workerId, string onceBackRunId)
        {
            IOnceWorker worker = await workerService.GetOnceWorkerByKey(workerId);
            if (worker == null)
                throw new BrunException(BrunErrorCode.NotFoundKey, $"can not find OnceWorker by key:'{workerId}'");
            worker.Run(onceBackRunId);
        }
        public async Task<IEnumerable<KeyValuePair<string, IBackRun>>> GetOnceBruns()
        {
            var result = new List<KeyValuePair<string, IBackRun>>();
            var workers = await workerService.GetAllOnceWorkers();
            foreach (OnceWorker item in workers)
            {
                foreach (var brun in item.BackRuns)
                {
                    result.Add(brun);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取当前程序所有可用的OnceBrun，供前端选择
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValueLabel> GetAllUserOnceBruns()
        {
            return backRunFilterService.GetOnceBackRunTypes().Select(m => new ValueLabel(m.FullName, m.Name));
        }


    }
}
