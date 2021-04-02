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
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        public override Task StartBrun(Type brunType)
        {
            BrunContext brunContext = new BrunContext(brunType);
            return taskFactory.StartNew(() =>
            {
                semaphoreSlim.Wait();
                Task executeTask = Execute(brunContext);
                executeTask.ContinueWith(t =>
                {
                    semaphoreSlim.Release();
                });
            });
        }
    }

}
