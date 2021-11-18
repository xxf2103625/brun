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
    public class TimeBackRunOption
    {
        //private string _id;
        public TimeBackRunOption(TimeSpan cycle, bool runWithStart=false, string backRunId = null)
        {
            Cycle = cycle;
            RunWithStart = runWithStart;
            if (backRunId == null)
            {
                Id = Guid.NewGuid().ToString();
            }
            else
            {
                Id = backRunId;
            }
        }
        public string Id { get; set; }
        public TimeSpan Cycle { get; set; }
        public bool RunWithStart { get; set; }
        public DateTime? NextTime { get; set; }
    }
}
