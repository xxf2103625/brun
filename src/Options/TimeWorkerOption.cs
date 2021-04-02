using Brun.BaskRuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    /// <summary>
    /// TimeWorker选项
    /// </summary>
    public class TimeWorkerOption : WorkerOption
    {
        public List<SimpleCycleTime> CycleTimes { get; set; }
    }
    public class SimpleCycleTime
    {
        public SimpleCycleTime()
        {
        }
        public SimpleCycleTime(IBackRun backRun, TimeSpan cycle, bool runWithStart)
        {
            BackRun = backRun;
            Cycle = cycle;
            RunWithStart = runWithStart;
        }
        public IBackRun BackRun { get; set; }
        public TimeSpan Cycle { get; set; }
        public bool RunWithStart { get; set; }
        public DateTime? NextTime { get; set; }
    }
}
