using Brun.BaskRuns;
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
    public class TimeBrunService : ITimeBrunService
    {
        IWorkerService workerService;
        public TimeBrunService(IWorkerService workerService)
        {
            this.workerService = workerService;
        }
        /// <summary>
        /// 添加TimeWorker
        /// </summary>
        /// <param name="timeWorker"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public ITimeWorker AddTimeBrun(ITimeWorker timeWorker, Type brunType, TimeBackRunOption option)
        {
            return ((TimeWorker)timeWorker).ProtectAddBrun(brunType, option);
        }
        /// <summary>
        /// 添加TimeWorker
        /// </summary>
        /// <typeparam name="TTimeBackRun"></typeparam>
        /// <param name="timeWorker"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public ITimeWorker AddTimeBrun<TTimeBackRun>(ITimeWorker timeWorker, TimeBackRunOption option) where TTimeBackRun : TimeBackRun
        {
            return this.AddTimeBrun(timeWorker, typeof(TTimeBackRun), option);
        }
        public IEnumerable<KeyValuePair<string, IBackRun>> GetTimeBruns()
        {
            var result = new List<KeyValuePair<string, IBackRun>>();
            var workers = workerService.GetAllTimeWorkers();
            foreach (TimeWorker item in workers)
            {
                foreach (var brun in item.BackRuns)
                {
                    result.Add(brun);
                }
            }
            return result;
        }
    }
}
