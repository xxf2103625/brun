using Brun.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Observers
{
    /// <summary>
    /// backrun运行拦截器
    /// </summary>
    public abstract class WorkerObserver
    {
        /// <summary>
        /// 构造函数，指定拦截的位置和顺序
        /// </summary>
        /// <param name="workerEvent"><see cref="WorkerEvents"/></param>
        /// <param name="order">越小先执行，默认100，100以内为组件内部拦截器</param>
        public WorkerObserver(WorkerEvents workerEvent, int order = 100)
        {
            this.Evt = workerEvent;
            Order = order;
        }
        public WorkerEvents Evt { get; }
        public int Order { get; }
        public abstract Task Todo(WorkerContext _context);
    }
}
