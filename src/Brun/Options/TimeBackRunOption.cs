using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// TimeBackRun配置
    /// </summary>
    public class TimeBackRunOption : BackRunOption
    {
        public TimeBackRunOption(TimeSpan cycle, bool runWithStart = false) : this(null, null, cycle, runWithStart)
        {
        }
        public TimeBackRunOption(string id, string name, TimeSpan cycle, bool runWithStart = false) : base(id, name)
        {
            Cycle = cycle;
            RunWithStart = runWithStart;
        }
        public TimeSpan Cycle { get; set; }
        public bool RunWithStart { get; set; }
        public DateTime? NextTime { get; set; }
    }
}
