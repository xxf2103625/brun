using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算PlanTime下次执行时间，计算流程需要单独控制，特殊情况会递归
    /// </summary>
    public abstract class BasePlanTimeComputer
    {
        protected TimeCloumn cloumn;
        protected TimeCloumnType timeCloumnType;

        public BasePlanTimeComputer(TimeCloumnType timeCloumnType)
        {
            this.timeCloumnType = timeCloumnType;
        }
        /// <summary>
        /// 计算下次执行时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="planTime"></param>
        /// <returns></returns>
        public virtual DateTimeOffset? Compute(DateTimeOffset? startTime, PlanTime planTime)
        {
            if (startTime == null)
                return null;
            cloumn = planTime.Times.FirstOrDefault(m => m.CloumnType == timeCloumnType);
            switch (cloumn.TimeStrategy)
            {
                case TimeStrategy.None:
                    throw new NotSupportedException($"the cloumn TimeStrategy is not supported {cloumn.TimeStrategy}");
                case TimeStrategy.Number:
                    startTime = Number(startTime.Value);
                    break;
                case TimeStrategy.Any:
                    startTime = Any(startTime.Value);
                    break;
                case TimeStrategy.And:
                    startTime = And(startTime.Value);
                    break;
                case TimeStrategy.To:
                    startTime = To(startTime.Value);
                    break;
                case TimeStrategy.Step:
                    startTime = Step(startTime.Value);
                    break;
                case TimeStrategy.Last:
                    startTime = Last(startTime.Value);
                    break;
                default:
                    throw new NotSupportedException($"the {cloumn.CloumnType} TimeStrategy is not supported {cloumn.TimeStrategy}");
                    //break;
            }
            return startTime;
        }
        /// <summary>
        /// L 最后n天 仅TimeCloumnType.Day中可用
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected abstract DateTimeOffset? Last(DateTimeOffset start);

        /// <summary>
        /// / 步进 10/5  */5  10-50/5
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected abstract DateTimeOffset? Step(DateTimeOffset start);
        /// <summary>
        /// - 范围 10-30
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected abstract DateTimeOffset? To(DateTimeOffset start);
        /// <summary>
        /// , 数组 2,5,8
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected abstract DateTimeOffset? And(DateTimeOffset start);
        /// <summary>
        /// * 任意
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected abstract DateTimeOffset? Any(DateTimeOffset start);
        /// <summary>
        /// 纯数字
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected abstract DateTimeOffset? Number(DateTimeOffset start);
    }
}
