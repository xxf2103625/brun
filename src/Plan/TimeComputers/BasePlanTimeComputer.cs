using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算PlanTime下次执行时间
    /// </summary>
    public abstract class BasePlanTimeComputer
    {
        /// <summary>
        /// 计算下次执行时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="timeCloumns"></param>
        /// <returns></returns>
        public abstract DateTimeOffset? Compute(DateTimeOffset? startTime, List<TimeCloumn> timeCloumns);
        /// <summary>
        /// 下一个域的计算器
        /// </summary>
        protected BasePlanTimeComputer nextComputer;
        /// <summary>
        /// 设置下一个域
        /// </summary>
        /// <param name="computer"></param>
        public void SetNext(BasePlanTimeComputer computer)
        {
            nextComputer = computer;
        }
    }
}
