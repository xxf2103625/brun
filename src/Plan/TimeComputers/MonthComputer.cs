using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算月
    /// </summary>
    public class MonthComputer : BasePlanTimeComputer
    {
        public MonthComputer() : base(TimeCloumnType.Month)
        {

        }
        protected override DateTimeOffset? And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(",");
            for (int i = 0; i < nbs.Length; i++)
            {
                int month = int.Parse(nbs[i]);
                //TODO 解析时按顺序储存
                if (month >= start.Month)
                {
                    return start.AddMonths(month - start.Month);
                }
            }
            int nextMonth = cloumn.Max - start.Month + int.Parse(nbs[0]);
            return start.AddMonths(nextMonth);
        }

        protected override DateTimeOffset? Any(DateTimeOffset start)
        {
            return start;
        }

        protected override DateTimeOffset? Number(DateTimeOffset start)
        {
            int begin = int.Parse(cloumn.Plan);
            if (start.Month <= begin)
            {
                return start.AddMonths(begin - start.Month);
            }
            else
            {
                int next = cloumn.Max - start.Month + begin;
                return start.AddMonths(next);
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
        private DateTimeOffset StepNb(DateTimeOffset start, int step, int begin, int end)
        {
            if (start.Month <= begin)
            {
                return start.AddMonths(begin - start.Month);
            }
            else //(start.Month > begin)
            {
                int nextMonth = begin;
                if (step == 0)
                {
                    throw new NotSupportedException("步进值不能为0,会死循环");
                }
                while (nextMonth <= end)
                {
                    if (nextMonth >= start.Month)
                    {
                        return start.AddMonths(nextMonth - start.Month);
                    }
                    nextMonth += step;
                }
                //下一年
                nextMonth = cloumn.Max  - start.Month + begin;
                return start.AddMonths(nextMonth);
            }
        }
        protected override DateTimeOffset? To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("-");
            int begin = int.Parse(nbs[0]);
            int end = int.Parse(nbs[1]);
            if (start.Month >= begin && start.Month <= end)
            {
                return start;
            }
            else if (start.Month > end)
            {
                int nextMonth = cloumn.Max  - start.Month + begin;
                return start.AddMonths(nextMonth);
            }
            else //start.Month<begin
            {
                return start.AddMonths(begin - start.Month);
            }
        }
    }
}
