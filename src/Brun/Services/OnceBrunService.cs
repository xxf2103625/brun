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
        //private readonly WorkerServer workerServer;
        //IBaseWorkerService<OnceWorker> baseService;
        IWorkerService workerService;
        IBackRunFilterService backRunFilterService;
        public OnceBrunService(IWorkerService workerService, IBackRunFilterService backRunFilterService)
        {
            //this.workerServer = workerServer;
            //this.baseService = baseWorkerService;
            this.workerService = workerService;
            this.backRunFilterService = backRunFilterService;
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
        public async Task<IEnumerable<KeyValuePair<string, IBackRun>>> GetOnceBruns()
        {
            var result = new List<KeyValuePair<string, IBackRun>>();
            var workers = await workerService.GetAllWorkers();
            foreach (var item in workers)
            {
                foreach (var brun in item.BackRuns)
                {
                    result.Add(brun);
                }
            }
            return result;
        }
        //public IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns(int current, int pageSize)
        //{
        //    return this.baseService.GetBackRuns().Skip((current-1)*pageSize);
        //}
        public IEnumerable<ValueLabel> GetAllUserOnceBruns()
        {
            return backRunFilterService.GetBackRunTypes().Select(m => new ValueLabel(m.FullName, m.Name));
        }

    }
}
