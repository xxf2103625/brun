using Brun.BaskRuns;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Services
{
    public interface IPlanBrunService
    {
        /// <summary>
        /// 添加PlanBackRun
        /// </summary>
        /// <param name="planWorker"></param>
        /// <param name="brunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        IPlanWorker AddPlanBrun(IPlanWorker planWorker, Type brunType, PlanBackRunOption option);
        /// <summary>
        /// 添加PlanBackRun
        /// </summary>
        /// <typeparam name="TPlanBackRun"></typeparam>
        /// <param name="planWorker"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        IPlanWorker AddPlanBrun<TPlanBackRun>(IPlanWorker planWorker, PlanBackRunOption option) where TPlanBackRun : PlanBackRun;
        IEnumerable<KeyValuePair<string, IBackRun>> GetPlanBruns();
    }
}