using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 简单的内存定时任务
    /// </summary>
    public class TimeWorker : AbstractWorker, ITimeWorker
    {
        public TimeWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
        }
        public Task Start()
        {
            throw new NotImplementedException();
        }
        public Task Pause()
        {
            throw new NotImplementedException();
        }

        public Task Resume()
        {
            throw new NotImplementedException();
        }


    }
}
