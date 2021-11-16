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
    public abstract class QueueBackRun : BackRun
    {
        QueueBackRunOption _option;
        public QueueBackRun(QueueBackRunOption option)
        {
            _option = option;
        }
        public override Task Run(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public abstract Task Run(string message, CancellationToken stoppingToken);
        public override string Id => _option.Id;
        public QueueBackRunOption Option => _option;
    }

}
