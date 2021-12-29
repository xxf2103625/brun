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
        /// <summary>
        /// 持久化模式用代码初始化时别用这个,每次会随机Id创建新的Worker
        /// </summary>
        public WorkerConfig() : this(null, null) { }
        /// <summary>
        /// 持久化模式用代码初始化时请保证key不要改变，不然每次启动程序会创建新的Worker
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        public WorkerConfig(string key, string name)
        {
            this.Key = key;
            this.Name = name;
            Init();
        }
        public void Init()
        {
            if (this.Key == null)
                this.Key = Guid.NewGuid().ToString();
            AddWorkerObserver(new List<WorkerObserver>()
            {
                new WorkerStartRunObserver(),
                new WorkerEndRunObserver(),
                new WorkerExceptObserver(),
            });
        }
        public string Key { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 内存中保存的最大异常数量
        /// </summary>
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
            return observers.Where(m => m.Evt == eventName);
        }
        public TimeSpan TimeWaitForBrun { get; set; } = TimeSpan.FromSeconds(2);
    }
}
