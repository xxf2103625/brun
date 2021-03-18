using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算天
    /// </summary>
    public class DayComputer : BasePlanTimeComputer
    {
        public DayComputer() : base(TimeCloumnType.Day)
        {

        }
        protected override DateTimeOffset? And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(",");
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
                start = start.AddMonths(1);//可能跨年
                return AddDaysFix(start, day);
            }
        }

        protected override DateTimeOffset? Step(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("/");
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
                    string[] tos = nbs[0].Split("-");
                    int begin = int.Parse(tos[0]);
                    int end = int.Parse(tos[1]);
                    return StepNb(start, step, begin, end);
                }
            }
        }
        private DateTimeOffset StepNb(DateTimeOffset start, int step, int begin, int end)
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
                while (nextDay <= end)
                {
                    if (nextDay >= start.Day)
                    {
                        return AddDaysFix(start, nextDay);
                    }
                    nextDay += step;
                }
                //下一月
                start = start.AddMonths(1);
                return AddDaysFix(start, begin);
            }
        }
        protected override DateTimeOffset? To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("-");
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
                start = start.AddMonths(1);
                return AddDaysFix(start, begin);
            }
            else //start.Day<begin
            {
                return AddDaysFix(start, begin);
            }
        }
        /// <summary>
        /// 本月Fix
        /// </summary>
        /// <param name="start"></param>
        /// <param name="planDay"></param>
        /// <returns></returns>
        private DateTimeOffset AddDaysFix(DateTimeOffset start, int planDay)
        {
            int maxDay = DateTime.DaysInMonth(start.Year, start.Month);
            if (planDay <= maxDay)
            {
                return new DateTimeOffset(start.Year, start.Month, planDay, start.Hour, start.Minute, start.Second, start.Offset);
            }
            else // 如果这个月没有31号,1 3 5 7 8 10 12必定31天，下个月一定满足
            {
                start = start.AddMonths(1);
                return new DateTimeOffset(start.Year, start.Month, planDay, start.Hour, start.Minute, start.Second, start.Offset);
            }
        }
    }
}
