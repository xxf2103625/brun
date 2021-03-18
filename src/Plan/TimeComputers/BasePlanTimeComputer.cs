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
        protected TimeCloumn cloumn;
        TimeCloumnType timeCloumnType;
        public BasePlanTimeComputer(TimeCloumnType timeCloumnType)
        {
            this.timeCloumnType = timeCloumnType;
        }
        /// <summary>
        /// 计算下次执行时间
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="timeCloumns"></param>
        /// <returns></returns>
        public virtual DateTimeOffset? Compute(DateTimeOffset? startTime, List<TimeCloumn> timeCloumns)
        {
            if (startTime == null)
                return null;
            //可能没有年
            if (timeCloumnType == TimeCloumnType.Year)
            {
                if (!timeCloumns.Any(m => m.CloumnType == timeCloumnType))
                    return startTime;
            }
            cloumn = timeCloumns.First(m => m.CloumnType == timeCloumnType);
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
                default:
                    throw new NotSupportedException($"the {cloumn.CloumnType} TimeStrategy is not supported {cloumn.TimeStrategy}");
                    //break;

            }
            if (nextComputer != null)
                return nextComputer.Compute(startTime, timeCloumns);
            else
                return startTime;
        }
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
