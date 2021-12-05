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
        public TimeBackRunOption(TimeSpan cycle, bool runWithStart) : base(null, null)
        {
            Cycle = cycle;
            RunWithStart = runWithStart;
        }
        public TimeBackRunOption(TimeSpan cycle, bool runWithStart, string id, string name) : base(id, name)
        {
            Cycle = cycle;
            RunWithStart = runWithStart;
        }

        //private string _id;
        //public TimeBackRunOption(TimeSpan cycle, bool runWithStart=false, string backRunId = null)
        //{
        //    Cycle = cycle;
        //    RunWithStart = runWithStart;
        //    if (backRunId == null)
        //    {
        //        Id = Guid.NewGuid().ToString();
        //    }
        //    else
        //    {
        //        Id = backRunId;
        //    }
        //}
        public TimeSpan Cycle { get; set; }
        public bool RunWithStart { get; set; }
        public DateTime? NextTime { get; set; }
    }
}
