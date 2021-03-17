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
        //private List<TimeCloumn> times;
        public PlanTime planTime;
        public PlanTimeComputer(PlanTime planTime)
        {
            this.planTime = planTime;
        }
        /// <summary>
        /// 计算PlanTime下次执行时间
        /// </summary>
        /// <param name="timeCloumns"></param>
        //public PlanTimeComputer(List<TimeCloumn> timeCloumns)
        //{
        //    times = timeCloumns;
        //}
        /// <summary>
        /// 获取下一次计划时间
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public DateTimeOffset? GetNextTime(DateTimeOffset start)
        {
            if (planTime.Times == null || planTime.Times.Count == 0)
            {
                throw new Exception("times is empty");
            }
            DateTimeOffset next = new DateTimeOffset(start.Year, start.Month, start.Day, start.Hour, start.Minute, start.Second + 1, start.Offset);
            //start.Second
            TimeCloumn cloumn = planTime.Times.First(m => m.CloumnType == TimeCloumnType.Second);
            return next;
        }
        private DateTimeOffset NextSecond(DateTimeOffset next, TimeCloumn cloumn)
        {
            switch (cloumn.TimeStrategy)
            {
                case TimeStrategy.Number:
                    break;

            }
            throw new NotImplementedException();
        }
        private DateTimeOffset GetNextNumber(DateTimeOffset next, TimeCloumn cloumn)
        {
            throw new NotImplementedException();
            if (int.TryParse(cloumn.Plan, out int nb))
            {
                if (next.Second == nb)
                {
                    return next;
                }
                if (next.Second < nb)
                {
                    return next.AddSeconds(nb - next.Second);
                }
                if (next.Second > nb)
                {
                    return next.AddSeconds(next.Second - nb);
                }
            }
            else
            {
                throw new Exception($"can not parse {cloumn.Plan}");
            }
        }
    }
}
