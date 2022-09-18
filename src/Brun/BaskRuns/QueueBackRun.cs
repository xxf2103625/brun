using Brun.BaskRuns;
using Brun.Options;
using System;
using System.Collections.Concurrent;
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
    public abstract class QueueBackRun : BackRun, IQueueRun
    {
        private ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        public QueueBackRun(QueueBackRunOption option) : base(option)
        {
        }
        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public abstract Task Run(string message, CancellationToken stoppingToken);
        /// <summary>
        /// 消息队列
        /// </summary>
        internal ConcurrentQueue<string> Queue => _queue;
        internal TimeBackRunOption Option => (TimeBackRunOption)option;
    }

}
