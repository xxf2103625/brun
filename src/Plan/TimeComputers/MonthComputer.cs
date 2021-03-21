using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算月
    /// </summary>
    public class MonthComputer : BasePlanTimeComputer
    {
        /// <summary>
        /// 特殊的日期，29，30，31，月和年变动的时候可能需要回到dayComputer重新计算
        /// </summary>
        //private bool isSpecialDay = false;
        /// <summary>
        /// 初始月，在这里变化后可能需要重新计算天
        /// </summary>
        private int initMonth;
        /// <summary>
        /// 初始年，这里可能跨年，也可能需要重新计算天
        /// </summary>
        private int initYear;
        private DateTimeOffset? _next;
        private bool goBack = false;
        public MonthComputer() : base(TimeCloumnType.Month)
        {

        }
        //TODO 重写返回逻辑，判断是否增加了月，增加了直接就返回去，简单粗暴。
        public override DateTimeOffset? Compute(DateTimeOffset? startTime, PlanTime planTime)
        {
            if (startTime != null)
            {
                this.goBack = false;
                initMonth = startTime.Value.Month;
                initYear = startTime.Value.Year;
                //int day = startTime.Value.Day;
                //if (day == 29 || day == 30 || day == 31)
                //    isSpecialDay = true;
            }
            this._next = base.Compute(startTime, planTime);
            if (_next != null)
            {
                if (initMonth != _next.Value.Month || initYear != _next.Value.Year)
                {
                    this.goBack = true;
                }
            }
            return this._next;
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
                    Thread.Sleep(5);
                }
                //下一年
                nextMonth = cloumn.Max - start.Month + begin;
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
                int nextMonth = cloumn.Max - start.Month + begin;
                return start.AddMonths(nextMonth);
            }
            else //start.Month<begin
            {
                return start.AddMonths(begin - start.Month);
            }
        }
        /// <summary>
        /// 是否回到Day重新计算
        /// </summary>
        //public bool ReturnToDay => isSpecialDay && (_next != null && _next.Value.Year != initYear && _next.Value.Month != initMonth);
        public bool GoBack => goBack;
        protected override DateTimeOffset? Last(DateTimeOffset start)
        {
            throw new NotImplementedException("just day can use L");
        }
    }
}
