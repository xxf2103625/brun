using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算天
    /// </summary>
    public class DayComputer : BasePlanTimeComputer
    {
        private DateTimeOffset? _next;
        private bool returnToDay = false;
        public DayComputer() : base(TimeCloumnType.Day)
        {

        }
        public override DateTimeOffset? Compute(DateTimeOffset? startTime, PlanTime planTime)
        {
            if (startTime == null)
                return null;
            returnToDay = false;
            this._next = base.Compute(startTime, planTime);
            return _next;
        }
        protected override DateTimeOffset? And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(',');
            for (int i = 0; i < nbs.Length; i++)
            {
                int day = int.Parse(nbs[i]);
                //TODO 解析时按顺序储存
                if (day >= start.Day)
                {
                    return AddDaysFix(start, day);
                }
            }
            int playDay = int.Parse(nbs[0]);
            start = start.AddMonths(1);//可能跨年
            return AddDaysFix(start, playDay);
        }

        protected override DateTimeOffset? Any(DateTimeOffset start)
        {
            return start;
        }
        /// <summary>
        /// 范围1-31，有些月只有28，30，29，没有就快进到下个月
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected override DateTimeOffset? Number(DateTimeOffset start)
        {
            int day = int.Parse(cloumn.Plan);
            if (start.Day <= day)//本月
            {
                return AddDaysFix(start, day);
            }
            else//下月
            {
                //纯数字可以直接加月
                start = start.AddMonths(1);//可能跨年
                return AddDaysFix(start, day);
            }
        }

        protected override DateTimeOffset? Step(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split('/');
            int step = int.Parse(nbs[1]);
            if (int.TryParse(nbs[0], out int nb))
            {
                return StepNb(start, step, nb, cloumn.Max);
            }
            else
            {
                if (nbs[0] == "*")
                {
                    return StepNb(start, step, 0, cloumn.Max);
                }
                else //包含范围的步进
                {
                    string[] tos = nbs[0].Split('-');
                    int begin = int.Parse(tos[0]);
                    int end = int.Parse(tos[1]);
                    return StepNb(start, step, begin, end);
                }
            }
        }
        private DateTimeOffset? StepNb(DateTimeOffset start, int step, int begin, int end)
        {
            if (start.Day <= begin)
            {
                return AddDaysFix(start, begin);
            }
            else //(start.Day > begin)
            {
                int nextDay = begin;
                if (step == 0)
                {
                    throw new NotSupportedException("步进值不能为0,会死循环");
                }
                //TODO 5/30、20/15会死循环
                while (nextDay <= end)
                {
                    if (nextDay >= start.Day)
                    {
                        return AddDaysFix(start, nextDay);
                    }
                    nextDay += step;
                    Thread.Sleep(5);
                }
                //下一月 日归零再算一次
                returnToDay = true;
                return start.AddDays(DateTime.DaysInMonth(start.Year,start.Month) - start.Day + 1);
            }
        }
        protected override DateTimeOffset? To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split('-');
            int begin = int.Parse(nbs[0]);//可能30，本月可能跳过
            int end = int.Parse(nbs[1]);//可能31，本月可能跳过
            if (start.Day >= begin && start.Day <= end)
            {
                //范围内，不可能超出
                return start;
            }
            else if (start.Day > end)
            {
                //极端 1月31号 30-31 next：3月30
                returnToDay = true;
                return start.AddDays(DateTime.DaysInMonth(start.Year, start.Month) - start.Day + 1);
            }
            else //start.Day<begin
            {
                return AddDaysFix(start, begin);
            }
        }
        /// <summary>
        /// 最后N天，L=最后一天 5L=最后一天再往前5天
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected override DateTimeOffset? Last(DateTimeOffset start)
        {
            if (cloumn.Plan == "L")
            {
                int lastDay = DateTime.DaysInMonth(start.Year, start.Month);
                return new DateTimeOffset(start.Year, start.Month, lastDay, start.Hour, start.Minute, start.Second, start.Offset);
            }
            else //5L  极端情况 30L 只有31天的月才触发
            {
                string nbStr = cloumn.Plan.Substring(0, cloumn.Plan.Length - 1);
                int nb = int.Parse(nbStr);
                int planDay = DateTime.DaysInMonth(start.Year, start.Month) - nb;
                if (planDay >= 1)
                {
                    if (planDay < start.Day)
                    {
                        start = start.AddMonths(1);
                        returnToDay = true;
                        //不能在这里直接加月，可能13月
                        return new DateTimeOffset(start.Year, start.Month, 1, start.Hour, start.Minute, start.Second, start.Offset);
                    }
                    return new DateTimeOffset(start.Year, start.Month, planDay, start.Hour, start.Minute, start.Second, start.Offset);
                }
                else
                {
                    start = start.AddMonths(1);
                    returnToDay = true;
                    //不能在这里直接加月，可能13月
                    return new DateTimeOffset(start.Year, start.Month, 1, start.Hour, start.Minute, start.Second, start.Offset);
                }
            }
        }
        /// <summary>
        /// 本月Fix
        /// </summary>
        /// <param name="start"></param>
        /// <param name="planDay"></param>
        /// <returns></returns>
        private DateTimeOffset? AddDaysFix(DateTimeOffset start, int planDay)
        {
            //单独处理2月 29号
            if (start.Month == 2 && planDay == 29)
            {
                if (!DateTime.IsLeapYear(start.Year))
                {
                    //goto day
                    returnToDay = true;
                    return new DateTimeOffset(start.Year, 3, 1, start.Hour, start.Minute, start.Second, start.Offset);
                }
            }
            int maxDay = DateTime.DaysInMonth(start.Year, start.Month);
            if (planDay <= maxDay)
            {
                return new DateTimeOffset(start.Year, start.Month, planDay, start.Hour, start.Minute, start.Second, start.Offset);
            }
            else
            {
                //不能直接加月 可能13月
                start = start.AddDays(maxDay - start.Day + 1);//到下月1号
                //goto day
                returnToDay = true;
                return start;
            }
        }

        public bool ReturnToDay => returnToDay;
    }
}
