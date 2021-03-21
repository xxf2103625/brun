﻿using Brun.Plan.TimeComputers;
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
