using Brun.BaskRuns;
using Brun.Options;
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
        //QueueBackRunOption _option;
        public QueueBackRun(QueueBackRunOption option):base(option)
        {
            //_option = option;
        }
        public override Task Run(CancellationToken stoppingToken)
        {
            throw new NotImplementedException("use Run(string message, CancellationToken stoppingToken)");
        }
        /// <summary>
        /// 业务逻辑
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public abstract Task Run(string message, CancellationToken stoppingToken);
        //public override string Id => _option.Id;
        public QueueBackRunOption Option => (QueueBackRunOption)option;
    }

}
