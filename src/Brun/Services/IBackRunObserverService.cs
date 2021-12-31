using Brun.Contexts;
using System.Threading.Tasks;

namespace Brun.Services
{
    public interface IBackRunObserverService
    {
        /// <summary>
        /// BackRun结束拦截器操作
        /// </summary>
        /// <param name="brunContext"></param>
        Task End(BrunContext brunContext);
        /// <summary>
        /// BackRun异常拦截器操作
        /// </summary>
        /// <param name="brunContext"></param>
        Task Except(BrunContext brunContext);
        ///// <summary>
        ///// 获取该BackRun异常数量(+1)
        ///// </summary>
        ///// <param name="brunId"></param>
        ///// <returns></returns>
        //long GetExceptNb(string brunId);
        ///// <summary>
        ///// 获取该BackRun开始序号
        ///// </summary>
        ///// <param name="brunId"></param>
        ///// <returns></returns>
        //long GetStartNb(string brunId);
        /// <summary>
        /// BackRun开始执行拦截器操作
        /// </summary>
        /// <param name="brunContext"></param>
        Task Start(BrunContext brunContext);
    }
}