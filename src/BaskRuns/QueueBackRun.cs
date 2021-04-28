using Brun.BaskRuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 队列任务基类
    /// </summary>
    public abstract class QueueBackRun : BackRunServicePrivoder, IQueueBackRun
    {
        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public abstract Task Run(string message, CancellationToken stoppingToken);
    }

}
