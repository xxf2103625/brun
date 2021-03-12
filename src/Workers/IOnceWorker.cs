using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public interface IOnceWorker : IWorker
    {
        /// <summary>
        /// 自定义数据
        /// </summary>
        /// <returns></returns>
        ConcurrentDictionary<string, string> GetData();

        string GetData(string key);
        /// <summary>
        /// 运行默认的BackRun
        /// </summary>
        /// <returns></returns>
        
        Task Run();
        /// <summary>
        /// 运行指定类型的BackRun
        /// </summary>
        /// <returns></returns>
        Task Run<TBackRun>();
        /// <summary>
        /// 运行指定类型的BackRun
        /// </summary>
        /// <param name="backRunType">backRun类型</param>
        /// <returns></returns>
        Task Run(Type backRunType);

        /// <summary>
        /// 直接运行不等待
        /// </summary>
        void RunDontWait();
        /// <summary>
        /// 直接运行不等待
        /// </summary>
        /// <typeparam name="TBackRun">其它的Brun</typeparam>
        void RunDontWait<TBackRun>();
    }
}
