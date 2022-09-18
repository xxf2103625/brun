using Brun.BaskRuns;
using Brun.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Services
{
    /// <summary>
    /// BackRun内存模式运行记录相关服务，注册单例,拦截器中使用
    /// </summary>
    public class BackRunObserverService : IBackRunObserverService
    {
        /// <summary>
        /// 内存模式中的BackRun运行记录集合
        /// </summary>
        private List<BrunContext> list => BackRunDetailService.BrunContexts;
        //TODO 配置最大保存数量
        private int MaxLength = 1000;
        public Task Start(BrunContext brunContext)
        {
            brunContext.StartNb = Interlocked.Increment(ref ((BackRun)brunContext.BackRun).startNb);
            if (list.Count > MaxLength)
            {
                list.RemoveAt(0);
            }
            list.Add(brunContext);
            return Task.CompletedTask;
        }
        /// <summary>
        /// 记录异常
        /// </summary>
        /// <param name="brunContext"></param>
        public Task Except(BrunContext brunContext)
        {
            //TODO 记录异常
            brunContext.ExceptNb = Interlocked.Increment(ref ((BackRun)brunContext.BackRun).errorNb);
            return Task.CompletedTask;
        }
        public Task End(BrunContext brunContext)
        {
            //结束
            Interlocked.Increment(ref ((BackRun)brunContext.BackRun).endNb);
            brunContext.IsEnd = true;
            return Task.CompletedTask;
        }
    }
}
