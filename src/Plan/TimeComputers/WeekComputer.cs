using System;
using System.Collections.Generic;

namespace Brun.Plan.TimeComputers
{
    //月或年变动必定要回来重新计算，重新回来时日期设置1号
    public class WeekComputer : BasePlanTimeComputer
    {
        //保存这里计算后的年月，如果是从月/年回来，肯定不一样
        private int initYear = 0;
        private int initMonth = 0;
        private DateTimeOffset? _next;
        public WeekComputer() : base(TimeCloumnType.Week)
        {
        }
        public override DateTimeOffset? Compute(DateTimeOffset? startTime, PlanTime planTime)
        {
            if (startTime == null)
                return null;
            if (initYear != 0 && initMonth != 0)
            {
                if (initYear != startTime.Value.Year || initMonth != startTime.Value.Month)
                {
                    //必定是因为月/年增加后返回，日要清零
                    startTime = new DateTimeOffset(startTime.Value.Year, startTime.Value.Month, 1, startTime.Value.Hour, startTime.Value.Minute, startTime.Value.Second, startTime.Value.Offset);
                }
            }
            _next = base.Compute(startTime, planTime);
            if (_next != null)
            {
                initYear = _next.Value.Year;
                initMonth = _next.Value.Month;
            }
            return _next;
        }
        protected override DateTimeOffset? And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(",");
            SortedSet<int> weeks = new();
            for (int i = 0; i < nbs.Length; i++)
            {
                weeks.Add(int.Parse(nbs[i]));
            }
            int nowWeek = (int)start.DayOfWeek;
            if (weeks.Max - 1 < nowWeek)
            {
                //下周
                return start.AddDays(7 - nowWeek + weeks.Min - 1);
            }
            //本周
            foreach (var item in weeks)
            {
                if (item - 1 >= nowWeek)
                {
                    return start.AddDays(item - 1 - nowWeek);
                }
            }
            throw new Exception("weekComputer unknow error.");
        }

        protected override DateTimeOffset? Any(DateTimeOffset start)
        {
            return start;
        }

        protected override DateTimeOffset? Last(DateTimeOffset start)
        {
            throw new NotImplementedException();
        }

        protected override DateTimeOffset? Number(DateTimeOffset start)
        {
            int planWeek = int.Parse(cloumn.Plan) - 1;//1-7=>0-6
            int nowWeek = (int)start.DayOfWeek;
            if (planWeek == nowWeek)
            {
                return start;
            }
            else// 可能跨月 
            {
                int addDays = 0;
                if (planWeek > nowWeek)
                {
                    //本周内
                    addDays = planWeek - nowWeek;
                }
                else
                {
                    //下周
                    addDays = 7 - nowWeek + planWeek;
                }
                return start.AddDays(addDays);
            }
        }

        protected override DateTimeOffset? Step(DateTimeOffset start)
        {
            throw new NotSupportedException();
        }

        protected override DateTimeOffset? To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("-");
            int minWeek = int.Parse(nbs[0]);
            int maxWeek = int.Parse(nbs[1]);
            if ((int)start.DayOfWeek < minWeek - 1)
            {
                return start.AddDays(minWeek - 1 - (int)start.DayOfWeek);
            }
            else if ((int)start.DayOfWeek >= minWeek - 1 && (int)start.DayOfWeek <= maxWeek - 1)
            {
                return start;
            }
            else //start.DayOfWeek>maxWeek-1
            {
                //超出范围，快进到下周1,再快进到minWeek    pre= maxWeek - 1 - (int)start.DayOfWeek + 1 + minWeek - 1
                return start.AddDays(maxWeek - (int)start.DayOfWeek + minWeek - 1);
            }
        }
    }
}
