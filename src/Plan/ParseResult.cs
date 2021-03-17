using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Plan
{
    /// <summary>
    /// 解析结果
    /// </summary>
    public class ParseResult
    {
        private bool isError;
        private List<TimeCloumn> timeCloumns = new List<TimeCloumn>();
        /// <summary>
        /// 错误
        /// </summary>
        public IList<KeyValuePair<int, string>> Errors { get; private set; }
        /// <summary>
        /// 添加异常信息
        /// </summary>
        /// <param name="n">第n个域的参数</param>
        /// <param name="msg">错误消息</param>
        public void AddError(int n, string msg)
        {
            isError = true;
            if (Errors == null)
                Errors = new List<KeyValuePair<int, string>>();
            Errors.Add(new KeyValuePair<int, string>(n, msg));
        }
        /// <summary>
        /// 添加TimeCloumn
        /// </summary>
        /// <param name="timeCloumn"></param>
        public void AddTimeCloumn(TimeCloumn timeCloumn)
        {
            this.timeCloumns.Add(timeCloumn);
        }
        /// <summary>
        /// 解析是否异常
        /// </summary>
        public bool IsError => isError;
        /// <summary>
        /// 结果
        /// </summary>
        public List<TimeCloumn> TimeCloumns => timeCloumns;
    }
}
