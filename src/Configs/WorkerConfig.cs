using Brun.Enums;
using Brun.Observers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brun
{
    /// <summary>
    /// Worker配置项
    /// </summary>
    public class WorkerConfig
    {
        private List<WorkerObserver> observers = new List<WorkerObserver>();
        public WorkerConfig()
        {
            Init();
        }
        public WorkerConfig(string key,string name)
        {
            this.Key = key;
            this.Name = name;
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
        public string Key { get; set; }
        public string Name { get; set; }
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
