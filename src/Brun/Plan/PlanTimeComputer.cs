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

        /// <summary>
        /// 计算下一次计划时间
        /// </summary>
        /// <param name="planTime">时间计划</param>
        /// <param name="start"></param>
        /// <returns>找不到或超出范围返回null</returns>
        public DateTimeOffset? GetNextTime(PlanTime planTime,DateTimeOffset start)
        {
            if (planTime == null | !planTime.IsSuccess || planTime.Times == null || planTime.Times.Count == 0)
            {
                throw new Exception("planTime is not ready or is error,please parse first or check errors.");
            }
            //DateTimeOffset start = planTime.Begin;
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
    }
}
