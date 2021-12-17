using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    /// <summary>
    /// 不同类型的Worker可能需要不同的run方法
    /// </summary>
    internal interface IRun
    {
        /// <summary>
        /// 自己的任务逻辑
        /// </summary>
        /// <param name="stoppingToken">任务结束信号量，当系统正常停止/工作中心停止时会首先发信号，使用该token可以手动停止/取消未完成任务，超时未结束的任务会强制结束</param>
        /// <returns></returns>
        Task Run(CancellationToken stoppingToken);
    }
}
