using Brun.Commons;
using Brun.Enums;
using Brun.Observers;
using Brun.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 同步Worker，同一个Worker内，同一时间执行backrun会强制排队运行
    /// //TODO 优化同步Worker
    /// </summary>
    public class SynchroWorker : OnceWorker
    {
        public SynchroWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            config.AddWorkerObserver(new SynchroBeforRunObserver());
        }
    }
}
