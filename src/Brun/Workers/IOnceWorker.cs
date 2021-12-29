using Brun.Models;
using Brun.Options;
using Brun.Workers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 瞬时任务的执行器
    /// </summary>
    public interface IOnceWorker
    {
        /// <summary>
        /// 自定义数据
        /// </summary>
        /// <returns></returns>
        ConcurrentDictionary<string, string> GetData();
        /// <summary>
        /// 获取Worker中的共享数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetData(string key);
        /// <summary>
        /// 运行默认的BackRun，添加的第一个BackRun为默认BackRun
        /// </summary>
        /// <returns></returns>
        void Run();
        /// <summary>
        /// 运行指定Id的BackRun
        /// </summary>
        /// <param name="brunId"></param>
        void Run(string brunId);
        /// <summary>
        /// 运行指定类型的BackRun
        /// </summary>
        /// <returns></returns>
        void Run<TBackRun>() where TBackRun : OnceBackRun;
        /// <summary>
        /// 运行指定类型的BackRun
        /// </summary>
        /// <param name="backRunType">backRun类型,必须继承自OnceBackRun</param>
        /// <returns></returns>
        void Run(Type backRunType);
        /// <summary>
        /// 添加自己实现的的OnceBackRun
        /// </summary>
        /// <param name="backRunType">必须继承自OnceBackRun</param>
        /// <param name="option"></param>
        /// <returns></returns>
        OnceBackRun AddBrun(Type backRunType, OnceBackRunOption option = null);
        /// <summary>
        /// 添加自己实现的的OnceBackRun
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <param name="option">为null时随机Id，Name为类型名称</param>
        /// <returns></returns>
        OnceBackRun AddBrun<TBackRun>(OnceBackRunOption option = null) where TBackRun : OnceBackRun;
    }
}
