using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算分
    /// </summary>
    public class MinuteComputer : BasePlanTimeComputer
    {
        public MinuteComputer() : base(TimeCloumnType.Minute)
        {

        }
        //纯数字
        protected override DateTimeOffset? Number(DateTimeOffset start)
        {
            int begin = int.Parse(cloumn.Plan);
            if (start.Minute <= begin)
            {
                return start.AddMinutes(begin - start.Minute);
            }
            else
            {
                int next = cloumn.Max + 1 - start.Minute + begin;
                return start.AddMinutes(next);
            }
        }
        /// <summary>
        /// *
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected override DateTimeOffset? Any(DateTimeOffset start)
        {
            return start;
        }
        /// <summary>
        /// ,
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected override DateTimeOffset? And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(",");
            for (int i = 0; i < nbs.Length; i++)
            {
                int minute = int.Parse(nbs[i]);
                //TODO 解析时按顺序储存
                if (minute >= start.Minute)
                {
                    return start.AddMinutes(minute - start.Minute);
                }
            }
            int nextMinute = cloumn.Max + 1 - start.Minute + int.Parse(nbs[0]);
            return start.AddMinutes(nextMinute);
        }
        /// <summary>
        /// -
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        protected override DateTimeOffset? To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("-");
            int begin = int.Parse(nbs[0]);
            int end = int.Parse(nbs[1]);
            if (start.Minute >= begin && start.Minute <= end)
            {
                return start;
            }
            else if (start.Minute > end)
            {
                int nextMinute = cloumn.Max + 1 - start.Minute + begin;
                return start.AddMinutes(nextMinute);
            }
            else //start.Minute<begin
            {
                return start.AddMinutes(begin - start.Minute);
            }
        }
        /// <summary>
        /// / 步进 第一个参数可能是数字或*或1-10
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
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
            if (start.Minute <= begin)
            {
                return start.AddMinutes(begin - start.Minute);
            }
            else //(start.Minute > begin)
            {
                int nextMinute = begin;
                if (step == 0)
                {
                    throw new NotSupportedException("步进值不能为0,会死循环");
                }
                while (nextMinute <= end)
                {
                    if (nextMinute >= start.Minute)
                    {
                        return start.AddMinutes(nextMinute - start.Minute);
                    }
                    nextMinute += step;
                    Thread.Sleep(5);
                }
                //下一小时
                nextMinute = cloumn.Max + 1 - start.Minute + begin;
                return start.AddMinutes(nextMinute);
            }
        }

        protected override DateTimeOffset? Last(DateTimeOffset start)
        {
            throw new NotImplementedException("just day can use L");
        }
    }
}
