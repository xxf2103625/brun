using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Services
{
    public class QueueBrunService : IQueueBrunService
    {
        IWorkerService workerService;
        IBackRunFilterService backRunFilterService;
        public QueueBrunService(IWorkerService workerService, IBackRunFilterService backRunFilterService)
        {
            this.workerService = workerService;
            this.backRunFilterService = backRunFilterService;
        }
        public virtual Task<IQueueWorker> AddQueueBrun(IQueueWorker queueWorker, Type queueBackRunType, QueueBackRunOption option)
        {
            return Task.FromResult((IQueueWorker)((QueueWorker)queueWorker).ProtectAddBrun(queueBackRunType, option));
        }
        public virtual Task<IQueueWorker> AddQueueBrun<TQueueBackRun>(IQueueWorker queueWorker, QueueBackRunOption option) where TQueueBackRun : QueueBackRun
        {
            return this.AddQueueBrun(queueWorker, typeof(TQueueBackRun), option);
        }
    }
}
