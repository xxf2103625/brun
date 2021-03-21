using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算年
    /// </summary>
    public class YearComputer : BasePlanTimeComputer
    {
        /// <summary>
        /// 特殊的日期，28,29，年变动的时候可能需要回到dayComputer重新计算
        /// </summary>
        //private bool isSpecialDay = false;
        private int initYear;
        private bool goBack=false;
        private DateTimeOffset? _next;
        public YearComputer() : base(TimeCloumnType.Year)
        {
        }
        //TODO 重写返回逻辑，判断是否增加了年，增加了直接就返回去，简单粗暴。
        public override DateTimeOffset? Compute(DateTimeOffset? startTime,PlanTime planTime)
        {
            if (startTime == null)
                return null;
            if (!planTime.Times.Any(m => m.CloumnType == TimeCloumnType.Year))
                return startTime;
            //if ((startTime.Value.Day == 29 || startTime.Value.Day == 28) && startTime.Value.Month == 2)
            //    this.isSpecialDay = true;
            this.goBack = false;
            initYear = startTime.Value.Year;
            this._next = base.Compute(startTime, planTime);
            if (_next != null)
            {
                if (initYear != _next.Value.Year)
                {
                    this.goBack = true;
                }
            }
            return _next;
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
            return null;
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
                    if (nextYear > cloumn.Max)
                    {
                        return null;
                    }
                    Thread.Sleep(5);
                }
                //超出范围
                return null;
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
                return null;
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
        /// <summary>
        /// 是否回到Day重新计算
        /// </summary>
        //public bool ReturnToDay => isSpecialDay && (_next != null && initYear != _next.Value.Year);
        public bool GoBack => goBack;
        protected override DateTimeOffset? Last(DateTimeOffset start)
        {
            throw new NotImplementedException("just day can use L");
        }
    }
}
