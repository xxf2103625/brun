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
    public class TimeWorkerService
    {
        IBaseWorkerService<TimeWorker> baseService;
        public TimeWorkerService(IBaseWorkerService<TimeWorker> baseWorkerService)
        {
            this.baseService = baseWorkerService;
        }
        /// <summary>
        /// 添加TimeWorker
        /// </summary>
        /// <param name="timeWorker"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public BrunResultState AddTimeBrun(TimeWorker timeWorker, Type brunType, TimeBackRunOption option)
        {
            return timeWorker.AddBrun(brunType, option);
        }
        public IEnumerable<KeyValuePair<string, IBackRun>> GetOnceBruns()
        {
            return this.baseService.GetBackRuns();
        }
    }
}
