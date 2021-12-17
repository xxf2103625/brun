using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    /// <summary>
    /// TimeBackRun配置
    /// </summary>
    public class TimeBackRunOption : BackRunOption
    {
        public TimeBackRunOption(TimeSpan cycle, bool runWithStart) : this(cycle, runWithStart, null, null)
        {
        }
        public TimeBackRunOption(TimeSpan cycle, bool runWithStart, string id, string name) : base(id, name)
        {
            Cycle = cycle;
            RunWithStart = runWithStart;
        }
        public TimeSpan Cycle { get; set; }
        public bool RunWithStart { get; set; }
        public DateTime? NextTime { get; set; }
    }
}
