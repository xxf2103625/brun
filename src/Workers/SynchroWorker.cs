using Brun.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 同步Worker
    /// </summary>
    public class SynchroWorker : AbstractWorker
    {
        
        public SynchroWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            config.AddWorkerObserver(new Observers.SynchroBeforRunObserver());
        }


        protected override Task Execute()
        {
           
            IBackRun backRun = (BackRun)BrunTool.CreateInstance(_option.BrunType);
            if (_context.Items != null && _option.BrunType.IsSubclassOf(typeof(BackRun)))
            {
                ((BackRun)backRun).Data = _context.Items;
            }
            return backRun.Run(WorkerServer.Instance.StoppingToken);
        }
    }
}
