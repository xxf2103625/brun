using System;
using System.Collections.Generic;
using System.Text;

namespace Brun.Models
{
    public class BrunInfo
    {
        public DateTime? StartTime { get; set; }
        public List<WorkerInfo> Workers { get; set; }
    }
}
