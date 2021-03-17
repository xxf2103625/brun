using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// 时间计划解析器
    /// </summary>
    public interface IPlanTimeParser
    {
        /// <summary>
        /// 解析计划表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        ParseResult Parse(string expression);
    }
}
