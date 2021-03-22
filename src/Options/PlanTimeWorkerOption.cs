using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    public class PlanTimeWorkerOption : WorkerOption
    {
        public Dictionary<Type, List<string>> planTimeRuns;
        public override Type DefaultBrunType => planTimeRuns.First().Key;
    }
}
