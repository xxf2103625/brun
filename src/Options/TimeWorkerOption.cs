using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    /// <summary>
    /// TimeWorker选项
    /// </summary>
    public class TimeWorkerOption : WorkerOption
    {
        /// <summary>
        /// 定时执行周期
        /// </summary>
        public TimeSpan Cycle { get; set; }
        /// <summary>
        /// 程序启动/重启时执行一次
        /// </summary>
        public bool RunWithStart { get; set; }
    }
}
