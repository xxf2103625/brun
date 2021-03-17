using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan.TimeComputers
{
    /// <summary>
    /// 计算秒
    /// </summary>
    public class SecondComputer : BasePlanTimeComputer
    {
        private TimeCloumn cloumn;
        /// <summary>
        /// 计算下一次
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="timeCloumns"></param>
        /// <returns></returns>
        public override DateTimeOffset? Compute(DateTimeOffset? startTime, List<TimeCloumn> timeCloumns)
        {
            if (startTime == null)
                return null;
            cloumn = timeCloumns.First(m => m.CloumnType == TimeCloumnType.Second);

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
        private DateTimeOffset Number(DateTimeOffset start)
        {
            int begin = int.Parse(cloumn.Plan);
            if (start.Second <= begin)
            {
                return start.AddSeconds(begin - start.Second);
            }
            else
            {
                int next = cloumn.Max + 1 - start.Second + begin;
                return start.AddSeconds(next);
            }
        }
        /// <summary>
        /// * 任意秒
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private DateTimeOffset Any(DateTimeOffset start)
        {
            //开始计算前已经+1s
            return start;//.AddSeconds(1);
        }
        /// <summary>
        /// , 和 1,5,10
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private DateTimeOffset And(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split(",");
            for (int i = 0; i < nbs.Length; i++)
            {
                int sec = int.Parse(nbs[i]);
                //TODO 解析时按顺序储存
                if (sec >= start.Second)
                {
                    return start.AddSeconds(sec - start.Second);
                }
            }
            int nextSec = cloumn.Max + 1 - start.Second + int.Parse(nbs[0]);
            return start.AddSeconds(nextSec);
        }
        /// <summary>
        /// - 范围 10-50
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        private DateTimeOffset To(DateTimeOffset start)
        {
            string[] nbs = cloumn.Plan.Split("-");
            int begin = int.Parse(nbs[0]);
            int end = int.Parse(nbs[1]);
            if (start.Second >= begin && start.Second <= end)
            {
                //计算前已经步进了1s
                return start;
            }
            else if (start.Second > end)
            {
                int nextSec = cloumn.Max + 1 - start.Second + begin;
                return start.AddSeconds(nextSec);
            }
            else //start.Second<begin
            {
                return start.AddSeconds(begin - start.Second);
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
            if (start.Second <= begin)
            {
                return start.AddSeconds(begin - start.Second);
            }
            else //(start.Second > begin)
            {
                int nextSec = begin;
                if (step == 0)
                {
                    throw new NotSupportedException("步进值不能为0,会死循环");
                }
                while (nextSec <= end)
                {
                    if (nextSec >= start.Second)
                    {
                        return start.AddSeconds(nextSec - start.Second);
                    }
                    nextSec += step;
                }
                //下一分钟
                nextSec = cloumn.Max + 1 - start.Second + begin;
                return start.AddSeconds(nextSec);
            }
        }
    }
}
