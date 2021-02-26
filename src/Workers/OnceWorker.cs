using Brun.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 基础Worker，每次执行一次
    /// </summary>
    public class OnceWorker : AbstractWorker
    {
        public OnceWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
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
