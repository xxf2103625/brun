using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public interface IPlanWorker : IWorker
    {
        /// <summary>
        /// 添加PlanBackRun
        /// </summary>
        /// <param name="planBackRunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        Task<IPlanWorker> AddBrun(Type planBackRunType, PlanBackRunOption option);
        Task<IPlanWorker> AddBrun<TPlanBackRun>(PlanBackRunOption option) where TPlanBackRun : PlanBackRun;
    }
}
