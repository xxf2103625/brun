using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// 时间计划
    /// </summary>
    public class PlanTime
    {
        //解析结果
        private ParseResult result;
        //解析后的域集合
        private List<TimeCloumn> times;
        //原始表达式
        private string expression;
        private DateTimeOffset begin;
        //解析器
        private IPlanTimeParser parser;
        /// <summary>
        /// 需要自己调用Parse方法
        /// </summary>
        /// <param name="planTimeParser"></param>
        public PlanTime(IPlanTimeParser planTimeParser = null)
        {
            if (planTimeParser == null)
                parser = WorkerServer.Instance.PlanTimeParser;
        }
        /// <summary>
        /// 内部已经调用Parse方法
        /// </summary>
        /// <param name="strExpression"></param>
        /// <param name="beginTime"></param>
        /// <param name="planTimeParser"></param>
        public PlanTime(string strExpression, DateTimeOffset? beginTime = null, IPlanTimeParser planTimeParser = null) : this(planTimeParser)
        {
            this.expression = strExpression;
            if (beginTime == null)
            {
                begin = DateTimeOffset.Now;
            }
            else
            {
                begin = beginTime.Value;
            }
            Parse(strExpression);
        }
        public PlanTime(IEnumerable<TimeCloumn> timeCloumns)
        {
            this.times = timeCloumns.ToList();
        }
        /// <summary>
        /// 解析时间计划表达式
        /// </summary>
        /// <param name="strExpression"></param>
        /// <returns>true：success，false：error</returns>
        public bool Parse(string strExpression)
        {
            this.expression = strExpression.Trim().ToUpper();
            this.result = parser.Parse(expression);
            if (result.IsError)
            {
                return false;
            }
            else
            {
                times = result.TimeCloumns;
                return true;
            }
        }
        /// <summary>
        /// 表达式字符串
        /// </summary>
        public string Expression => expression;
        public DateTimeOffset Begin => begin;
        /// <summary>
        /// 是否已解析
        /// </summary>
        public bool IsParsed => result != null;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess => result != null && !result.IsError;
        /// <summary>
        /// 解析失败的异常信息
        /// </summary>
        public IList<KeyValuePair<int, string>> Errors => result.Errors;
        /// <summary>
        /// 解析后的结果，仅储存原始字符串和计划策略
        /// </summary>
        public List<TimeCloumn> Times => times;
        public bool IsWeek => result.IsWeek;
    }
}
