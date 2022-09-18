using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// 表达式域的类型 yyyy-MM-dd HH:mm:ss 
    /// </summary>
    public enum TimeCloumnType
    {
        /// <summary>
        /// 默认值，没有分配到任何列
        /// </summary>
        None,
        /// <summary>
        /// 秒
        /// </summary>
        Second,
        /// <summary>
        /// 分钟
        /// </summary>
        Minute,
        /// <summary>
        /// 小时
        /// </summary>
        Hour,
        /// <summary>
        /// 天
        /// </summary>
        Day,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 星期
        /// </summary>
        Week,
        /// <summary>
        /// 年
        /// </summary>
        Year
    }
}
