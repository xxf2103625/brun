using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 计划时间任务
    /// </summary>
    public abstract class PlanBackRun : BackRun
    {
        //private PlanBackRunOption _option;
        public PlanBackRun(PlanBackRunOption option) : base(option)
        {
            //_option = option;
        }
        public PlanBackRunOption Option => (PlanBackRunOption)option;
        //public override string Id => _option.Id;
        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTimeOffset? LastRunTime { get; set; }
        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTimeOffset? NextRunTime { get; set; }
        //public void SetPlanBackRun(PlanBackRunOption option)
        //{
        //    this._option = option;
        //}
    }
}
