using Brun.BaskRuns;
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
        public virtual IQueueWorker AddQueueBrun(IQueueWorker queueWorker, Type queueBackRunType, QueueBackRunOption option)
        {
            return (IQueueWorker)((QueueWorker)queueWorker).ProtectAddBrun(queueBackRunType, option);
        }
        public virtual IQueueWorker AddQueueBrun<TQueueBackRun>(IQueueWorker queueWorker, QueueBackRunOption option) where TQueueBackRun : QueueBackRun
        {
            return this.AddQueueBrun(queueWorker, typeof(TQueueBackRun), option);
        }
        public IEnumerable<KeyValuePair<string, IBackRun>> GetQueueBruns()
        {
            var result = new List<KeyValuePair<string, IBackRun>>();
            var workers = workerService.GetAllQueueWorkers();
            foreach (QueueWorker item in workers)
            {
                foreach (var brun in item.BackRuns)
                {
                    result.Add(brun);
                }
            }
            return result;
        }
    }
}
