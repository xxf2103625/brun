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
        /// 运行，web中不要await
        /// </summary>
        /// <returns></returns>
        Task Run();
        /// <summary>
        /// 直接运行不用等待,适合web中使用
        /// </summary>
        void RunDontWait();
    }
}
