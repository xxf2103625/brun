using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    public class TimeWorkerOption:WorkerOption
    {
        public TimeSpan Cycle { get; set; }
    }
}
