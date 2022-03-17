using Brun.Contexts;
using Brun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// BackRun运行历史相关服务
    /// </summary>
    public class BackRunDetailService : IBackRunDetailService
    {
        /// <summary>
        /// 内存模式中的BackRun运行记录集合
        /// </summary>
        public static List<BrunContext> BrunContexts = new List<BrunContext>();
        public BackRunDetailService()
        {
        }
        /// <summary>
        /// 获取指定BackRunId的运行数量信息
        /// </summary>
        /// <param name="backRunId"></param>
        /// <returns></returns>
        public BackRunContextNumberModel GetBackRunDetailNumber(string backRunId)
        {
            BrunContext brunContext = BrunContexts.Where(m => m.BrunId == backRunId).OrderByDescending(m => m.Ct).FirstOrDefault();
            var lastErrorContext = BrunContexts.Where(m => m.BrunId == backRunId && m.Exception != null).OrderByDescending(m => m.Ct).FirstOrDefault();
            long errorNumber = 0;
            if (lastErrorContext != null)
                errorNumber = lastErrorContext.ExceptNb;
            if (brunContext == null)
            {
                return new BackRunContextNumberModel();
            }
            var r = new BackRunContextNumberModel()
            {
                BackRunId = brunContext.BrunId,
                Start = brunContext.StartNb,
                Except = errorNumber,
                Running = BrunContexts.Where(m => m.BrunId == backRunId && !m.IsEnd).LongCount()
            };
            return r;
        }
        public List<BackRunContextNumberModel> GetBackRunDetailNumbers(List<string> brunIds)
        {
            throw new NotImplementedException();
        }
    }
}
