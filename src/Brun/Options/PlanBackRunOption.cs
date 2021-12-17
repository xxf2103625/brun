using Brun.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    public class PlanBackRunOption : BackRunOption
    {
        public PlanBackRunOption(PlanTime planTime) : this(planTime, null, null)
        {
        }
        public PlanBackRunOption(PlanTime planTime, string id, string name) : base(id, name)
        {
            this.PlanTime = planTime;
        }
        public PlanTime PlanTime { get; set; }
    }
}
