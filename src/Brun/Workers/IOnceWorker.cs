using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public interface IOnceWorker
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

        void Run();
        /// <summary>
        /// 运行指定类型的BackRun
        /// </summary>
        /// <returns></returns>
        void Run<TBackRun>();
        /// <summary>
        /// 运行指定类型的BackRun
        /// </summary>
        /// <param name="backRunType">backRun类型</param>
        /// <returns></returns>
        void Run(Type backRunType);
    }
}
