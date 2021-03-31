using Brun.Commons;
using Brun.Contexts;
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
    /// 同步Worker，同一个Worker内，backrun串行运行
    /// </summary>
    public class SynchroWorker : OnceWorker
    {
        public SynchroWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
        }

        public override Task StartBrun(Type brunType)
        {
            BrunContext brunContext = new BrunContext(brunType);

            return Execute(brunContext);
        }
    }

}
