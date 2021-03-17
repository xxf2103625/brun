using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算分
    /// </summary>
    public class MinuteComputer : BasePlanTimeComputer
    {
        private TimeCloumn cloumn;
        public override DateTimeOffset? Compute(DateTimeOffset? startTime, List<TimeCloumn> timeCloumns)
        {
            if (startTime == null)
                return null;
            cloumn = timeCloumns.First(m => m.CloumnType == TimeCloumnType.Minute);

            switch (cloumn.TimeStrategy)
            {
                case TimeStrategy.Number:
                    startTime = Number(startTime.Value);
                    break;
                case TimeStrategy.Any:
                    startTime = Any(startTime.Value);
                    break;
                case TimeStrategy.And:
                    startTime = And(startTime.Value);
                    break;
                case TimeStrategy.To:
                    startTime = To(startTime.Value);
                    break;
                case TimeStrategy.Step:
                    startTime = Step(startTime.Value);
                    break;
                default:
                    throw new NotSupportedException($"the {cloumn.CloumnType} TimeStrategy is not supported {cloumn.TimeStrategy}");
                    //break;

            }
            if (nextComputer != null)
                return nextComputer.Compute(startTime, timeCloumns);
            else
                return startTime;
        }
        //纯数字
        private DateTimeOffset Number(DateTimeOffset start)
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
        private DateTimeOffset Any(DateTimeOffset start)
        {
            return start;
        }
        /// <summary>
        /// ,
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private DateTimeOffset And(DateTimeOffset start)
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
        private DateTimeOffset To(DateTimeOffset start)
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
        private DateTimeOffset Step(DateTimeOffset start)
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
                }
                //下一小时
                nextMinute = cloumn.Max + 1 - start.Minute + begin;
                return start.AddMinutes(nextMinute);
            }
        }
    }
}
