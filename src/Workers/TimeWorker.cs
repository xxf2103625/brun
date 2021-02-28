using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public class TimeWorker : AbstractWorker, ITimeWorker
    {
        public TimeWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
        }
        public Task Start()
        {
            throw new NotImplementedException();
        }
    }
}
