using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算年
    /// </summary>
    public class YearComputer : BasePlanTimeComputer
    {
        public YearComputer() : base(TimeCloumnType.Year)
        {
        }
        protected override DateTimeOffset? And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(",");
            for (int i = 0; i < nbs.Length; i++)
            {
                int year = int.Parse(nbs[i]);
                //TODO 解析时按顺序储存
                if (year >= start.Year)
                {
                    return start.AddYears(year - start.Year);
                }
            }
            int nextYear = cloumn.Max - start.Year + int.Parse(nbs[0]);
            return start.AddYears(nextYear);
        }

        protected override DateTimeOffset? Any(DateTimeOffset start)
        {
            if (start.Year > cloumn.Max)
                return null;
            return start;
        }

        protected override DateTimeOffset? Number(DateTimeOffset start)
        {
            int begin = int.Parse(cloumn.Plan);
            if (start.Year <= begin)
            {
                return start.AddYears(begin - start.Year);
            }
            else
            {
                return null;
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
                else
                {
                    string[] tos = nbs[0].Split("-");
                    int begin = int.Parse(tos[0]);
                    int end = int.Parse(tos[1]);
                    return StepNb(start, step, begin, end);
                }
            }
        }
        private DateTimeOffset? StepNb(DateTimeOffset start, int step, int begin, int end)
        {
            if (begin > cloumn.Max)
            {
                return null;
            }
            if (start.Year <= begin)
            {
                return start.AddYears(begin - start.Year);
            }
            else //(start.Year > begin)
            {
                int nextYear = begin;
                if (step == 0)
                {
                    throw new NotSupportedException("步进值不能为0,会死循环");
                }
                while (nextYear <= end)
                {
                    if (nextYear >= start.Year)
                    {
                        return start.AddYears(nextYear - start.Year);
                    }
                    nextYear += step;
                }
                //下一年
                if (nextYear > cloumn.Max)
                {
                    return null;
                }
                nextYear = cloumn.Max - start.Year + begin;
                if (nextYear > cloumn.Max)
                {
                    return null;
                }
                return start.AddYears(nextYear);
            }
        }
        protected override DateTimeOffset? To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("-");
            int begin = int.Parse(nbs[0]);
            int end = int.Parse(nbs[1]);
            if (start.Year >= begin && start.Year <= end)
            {
                return start;
            }
            else if (start.Year > end)
            {
                int nextYear = cloumn.Max - start.Year + begin;
                if (nextYear > cloumn.Max)
                {
                    return null;
                }
                return start.AddYears(nextYear);
            }
            else //start.Year<begin
            {
                if (begin > cloumn.Max)
                {
                    return null;
                }
                return start.AddYears(begin - start.Year);
            }
        }
    }
}
