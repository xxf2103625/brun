using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 小时
    /// </summary>
    public class HourComputer : BasePlanTimeComputer
    {
        /// <summary>
        /// 
        /// </summary>
        public HourComputer() : base(TimeCloumnType.Hour)
        {

        }
        protected override DateTimeOffset? And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(",");
            for (int i = 0; i < nbs.Length; i++)
            {
                int hour = int.Parse(nbs[i]);
                //TODO 解析时按顺序储存
                if (hour >= start.Hour)
                {
                    return start.AddHours(hour - start.Hour);
                }
            }
            int nextHour = cloumn.Max + 1 - start.Hour + int.Parse(nbs[0]);
            return start.AddHours(nextHour);
        }

        protected override DateTimeOffset? Any(DateTimeOffset start)
        {
            return start;
        }

        protected override DateTimeOffset? Number(DateTimeOffset start)
        {
            int begin = int.Parse(cloumn.Plan);
            if (start.Hour <= begin)
            {
                return start.AddHours(begin - start.Hour);
            }
            else
            {
                int next = cloumn.Max + 1 - start.Hour + begin;
                return start.AddHours(next);
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
            if (start.Hour <= begin)
            {
                return start.AddHours(begin - start.Hour);
            }
            else //(start.Hour > begin)
            {
                int nextHour = begin;
                if (step == 0)
                {
                    throw new NotSupportedException("步进值不能为0,会死循环");
                }
                while (nextHour <= end)
                {
                    if (nextHour >= start.Hour)
                    {
                        return start.AddHours(nextHour - start.Hour);
                    }
                    nextHour += step;
                }
                //下一天
                nextHour = cloumn.Max + 1 - start.Hour + begin;
                return start.AddHours(nextHour);
            }
        }
        protected override DateTimeOffset? To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("-");
            int begin = int.Parse(nbs[0]);
            int end = int.Parse(nbs[1]);
            if (start.Hour >= begin && start.Hour <= end)
            {
                return start;
            }
            else if (start.Hour > end)
            {
                int nextHour = cloumn.Max + 1 - start.Hour + begin;
                return start.AddHours(nextHour);
            }
            else //start.Hour<begin
            {
                return start.AddHours(begin - start.Hour);
            }
        }
    }
}
