using Brun.Options;
using Brun.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public class PlanBackRunOption : BackRunOption
    {
        public PlanBackRunOption(PlanTime planTime) : this(null, null, planTime)
        {
        }
        public PlanBackRunOption(string id, string name, PlanTime planTime) : base(id, name)
        {
            this.PlanTime = planTime;
        }
        public PlanTime PlanTime { get; set; }
    }
}
