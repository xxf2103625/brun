using Brun.Plan.TimeComputers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// 计算PlanTime下次执行时间
    /// </summary>
    public class PlanTimeComputer
    {
        private SecondComputer secondComputer = new SecondComputer();
        private MinuteComputer minuteComputer = new MinuteComputer();
        private HourComputer hourComputer = new HourComputer();
        private DayComputer dayComputer = new DayComputer();
        private WeekComputer weekComputer = new WeekComputer();
        private MonthComputer monthComputer = new MonthComputer();
        private YearComputer yearComputer = new YearComputer();
        private DateTimeOffset? lastTime;
        private DateTimeOffset? nextTime;
        //private BasePlanTimeComputer timeComputer;
        private PlanTime planTime;
        /// <summary>
        /// 需要自己SetPlanTime
        /// </summary>
        public PlanTimeComputer()
        {
        }
        /// <summary>
        /// 构造函数传入解析好的PlanTime
        /// </summary>
        /// <param name="planTime"></param>
        public PlanTimeComputer(PlanTime planTime)
        {
            this.planTime = planTime;
        }
        /// <summary>
        /// 传入解析好的PlanTime
        /// </summary>
        /// <param name="planTime"></param>
        public void SetPlanTime(PlanTime planTime)
        {
            this.planTime = planTime;
        }
        /// <summary>
        /// 计算下一次执行时间,如果上次执行时间为null，则以当前时间为基准
        /// </summary>
        /// <returns>找不到或超出范围返回null</returns>
        public DateTimeOffset? GetNextTime()
        {
            if (lastTime == null)
            {
                DateTimeOffset t = DateTime.Now;
                return GetNextTime(new DateTimeOffset(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, t.Offset));
            }
            else
                return GetNextTime(lastTime.Value);
        }
        /// <summary>
        /// 上一次执行时间，没有为null
        /// </summary>
        public DateTimeOffset? LastTime => lastTime;
        /// <summary>
        /// 获取下一次执行时间，没有为null
        /// </summary>
        public DateTimeOffset? NextTime => nextTime;
        /// <summary>
        /// 对应的PlanTime
        /// </summary>
        public PlanTime PlanTime => planTime;
        /// <summary>
        /// 设置上一次执行时间
        /// </summary>
        /// <param name="t"></param>
        public void SetLastTime(DateTimeOffset t)
        {
            this.lastTime = new DateTimeOffset(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, t.Offset);
        }
        /// <summary>
        /// 设置下一次执行时间
        /// </summary>
        /// <param name="t"></param>
        public void SetNextTime(DateTimeOffset t)
        {
            this.nextTime = new DateTimeOffset(t.Year, t.Month, t.Day, t.Hour, t.Minute, t.Second, t.Offset);
        }
        /// <summary>
        /// 计算下一次计划时间
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <returns>找不到或超出范围返回null</returns>
        public DateTimeOffset? GetNextTime(DateTimeOffset start)
        {
            if (planTime == null | !planTime.IsSuccess || planTime.Times == null || planTime.Times.Count == 0)
            {
                throw new Exception("planTime is not ready or is error,please parse first or check errors.");
            }
            //开始前就加1秒
            start = start.AddSeconds(1);
            //需要手动控制计算流程
            DateTimeOffset? next = secondComputer.Compute(start, planTime);
            next = minuteComputer.Compute(next, planTime);
            next = hourComputer.Compute(next, planTime);

        returnToDay://重新确认

            if (!planTime.IsWeek)
            {
                next = dayComputer.Compute(next, planTime);
                if (dayComputer.ReturnToDay)
                {
                    goto returnToDay;
                }
            }
            else
            {
                next = weekComputer.Compute(next, planTime);
            }
            next = monthComputer.Compute(next, planTime);
            if (monthComputer.GoBack)
            {
                goto returnToDay;
            }
            next = yearComputer.Compute(next, planTime);
            if (yearComputer.GoBack)
            {
                goto returnToDay;
            }
            return next;
        }
        /// <summary>
        /// 重写相等判断，以PlanTime.Expression是否相同为标准（大写），PlanTime=null永不相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            PlanTimeComputer other = (PlanTimeComputer)obj;
            if (other.planTime == null || this.planTime == null)
            {
                return false;
            }
            else
            {
                if (other.planTime.Expression == this.planTime.Expression)
                {
                    return true;
                }
            }
            return false;
        }
        public override string ToString()
        {
            if (planTime == null || !planTime.IsSuccess)
            {
                return null;
            }
            else
            {
                return planTime.Expression;
            }
        }

        public override int GetHashCode()
        {
            if (planTime == null || !planTime.IsSuccess)
            {
                return base.GetHashCode();
            }
            return this.planTime.Expression.GetHashCode();
        }
    }
}
