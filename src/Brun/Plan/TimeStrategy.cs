using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// 表达式域解析策略
    /// , - * / ...的各种策略
    /// </summary>
    public enum TimeStrategy
    {
        /// <summary>
        /// 默认值，没有任何策略
        /// </summary>
        None,
        /// <summary>
        /// 纯数字
        /// </summary>
        Number,
        /// <summary>
        /// , 数组 0,15,45
        /// </summary>
        And,
        /// <summary>
        /// - 范围 25-45
        /// </summary>
        To,
        /// <summary>
        /// * 任何 每(秒/天)
        /// </summary>
        Any,
        /// <summary>
        /// / 步进 number或*(等同0)或范围/步进值
        /// </summary>
        Step,
        /// <summary>
        /// L 只能在日期域中出现，最后一天，5L：当月最后一天再倒数5天
        /// </summary>
        Last,
    }
}
