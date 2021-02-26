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
        private static object SynchroRun_LOCK = new object();
        public SynchroWorker(WorkerOption option = null, WorkerConfig config = null) : base(option, config)
        {
        }


        protected override Task Execute()
        {
            if (Context.endNb < Context.startNb)
            {
                lock (SynchroRun_LOCK)
                {
                    if (Context.endNb < Context.startNb)
                    {
                        Thread.Sleep(5);
                    }
                }
            }
            IBackRun backRun = (BackRun)BrunTool.CreateInstance(_option.BrunType);
            if (_context.Items != null && _option.BrunType.IsSubclassOf(typeof(BackRun)))
            {
                ((BackRun)backRun).Data = _context.Items;
            }
            return backRun.Run(WorkerServer.Instance.StoppingToken);
        }
    }
}
