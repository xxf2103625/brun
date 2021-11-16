using Brun.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    public class PlanBackRunOption
    {
        public PlanBackRunOption()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public PlanBackRunOption(PlanTime planTime, string id = null)
        {
            this.PlanTime = planTime;
            if (id != null)
                this.Id = id;
            else
                this.Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public PlanTime PlanTime { get; set; }

    }
}
