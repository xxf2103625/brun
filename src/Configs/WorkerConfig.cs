using Brun.Enums;
using Brun.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brun
{
    public class WorkerConfig
    {
        private List<WorkerObserver> observers = new List<WorkerObserver>();
        public WorkerConfig()
        {
            Init();
        }
        public void Init()
        {
            AddWorkerObserver(new List<WorkerObserver>()
            {
                new WorkerStartRunObserver(),
                new WorkerEndRunObserver(),
                new WorkerExceptObserver(),
            });
        }
        public int WorkerContextMaxExcept { get; set; } = 10;
        public void AddWorkerObserver(WorkerObserver workerObserver)
        {
            observers.Add(workerObserver);
        }
        public void AddWorkerObserver(IEnumerable<WorkerObserver> workerObservers)
        {
            observers.AddRange(workerObservers);
        }
        public IEnumerable<WorkerObserver> GetAllObservers()
        {
            return observers;
        }
        public IEnumerable<WorkerObserver> GetObservers(WorkerEvents eventName)
        {
            var list = observers.Where(m => m.Evt == eventName);
            return list;
        }
        public TimeSpan TimeWaitForBrun { get; set; } = TimeSpan.FromSeconds(2);
    }
}
