using Brun.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// BackRun运行记录相关服务,拦截器中使用
    /// </summary>
    public class BackRunObserverService : IBackRunObserverService
    {
        /// <summary>
        /// 内存模式中的BackRun运行记录集合
        /// </summary>
        private List<BrunContext> list => BackRunDetailService.BrunContexts;
        public void Start(BrunContext brunContext)
        {
            brunContext.StartNb = this.GetStartNb(brunContext.BrunId) + 1;
            list.Add(brunContext);
        }
        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="brunContext"></param>
        public void Except(BrunContext brunContext)
        {
            //TODO 记录异常
            brunContext.ExceptNb = this.GetExceptNb(brunContext.BrunId);
        }
        public void End(BrunContext brunContext)
        {
            //结束
            brunContext.IsEnd = true;
        }
        public long GetStartNb(string brunId)
        {
            return list.Where(m => m.BrunId == brunId).Count();
        }
        public long GetExceptNb(string brunId)
        {
            return list.Where(m => m.BrunId == brunId && m.Exception != null).LongCount();
        }
    }
}
