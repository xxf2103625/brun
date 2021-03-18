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
        private BasePlanTimeComputer timeComputer;
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
        /// 以当前时间准基础，计算下一次执行时间
        /// </summary>
        /// <returns>找不到或超出范围返回null</returns>
        public DateTimeOffset? GetNextTime()
        {
            return GetNextTime(DateTime.Now);
        }
        /// <summary>
        /// 获取下一次计划时间
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
            DateTimeOffset? next = TimeComputer.Compute(start, planTime.Times);
            return next;
        }
        private BasePlanTimeComputer TimeComputer
        {
            get
            {
                if (timeComputer == null)
                {
                    SecondComputer secondComputer = new SecondComputer();
                    MinuteComputer minuteComputer = new MinuteComputer();
                    HourComputer hourComputer = new HourComputer();
                    DayComputer dayComputer = new DayComputer();
                    MonthComputer monthComputer = new MonthComputer();
                    YearComputer yearComputer = new YearComputer();
                    secondComputer.SetNext(minuteComputer);
                    minuteComputer.SetNext(hourComputer);
                    hourComputer.SetNext(dayComputer);
                    dayComputer.SetNext(monthComputer);
                    monthComputer.SetNext(yearComputer);

                    timeComputer = secondComputer;
                }
                return timeComputer;
            }
        }
    }
}
