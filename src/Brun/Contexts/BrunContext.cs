using Brun.BaskRuns;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Contexts
{
    /// <summary>
    /// 每个BackRun运行时的上下文
    /// </summary>
    public class BrunContext : IDisposable
    {
        private readonly string id;
        private readonly IBackRun backRun;
        private IServiceProvider serviceProvider;
        private IServiceScope scope;
        private Type brunType;
        public BrunContext(IBackRun backRun)
        {
            this.id = Guid.NewGuid().ToString();
            this.Ct = DateTime.Now;
            this.backRun = backRun;
            this.brunType = backRun.GetType();
            this.serviceProvider = WorkerServer.Instance.ServiceProvider;
        }
        /// <summary>
        /// 每次运行创建一个随机Id
        /// </summary>
        public string Id => id;
        /// <summary>
        /// 所属Worker实例的Id
        /// </summary>
        public string WorkerId => backRun.WorkerContext.Key;
        /// <summary>
        /// BackRun的Id
        /// </summary>
        public string BrunId => backRun.Id;
        /// <summary>
        /// BackRun的Name
        /// </summary>
        public string BrunName => backRun.Name;
        /// <summary>
        /// BackRun的类型
        /// </summary>
        public Type BrunType => brunType;
        /// <summary>
        /// 开始序号
        /// </summary>
        public long StartNb { get; set; }
        /// <summary>
        /// 异常序号
        /// </summary>
        public long ExceptNb { get; set; }
        /// <summary>
        /// 是否执行结束
        /// </summary>
        public bool IsEnd { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { get; set; }
        /// <summary>
        /// QueueBackRun中传入的Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 是否异常
        /// </summary>
        public bool IsError => this.Exception != null;
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Ct { get; set; }
        /// <summary>
        /// 异常信息，没有为null
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// BackRun的实例
        /// </summary>
        public IBackRun BackRun => backRun;
        /// <summary>
        /// 当前Worker上下文
        /// </summary>
        public WorkerContext WorkerContext => BackRun.WorkerContext;
        /// <summary>
        /// Ioc容器
        /// </summary>
        public IServiceProvider ServiceProvider => WorkerContext.ServiceProvider;
        /// <summary>
        /// 持久化需要创建Scope来保证拦截器中的事务一致性
        /// </summary>
        public IServiceScope ServiceScope => scope;
        /// <summary>
        /// 创建新的ServiceScope，保证执行一次任务的事务一致性
        /// </summary>
        public IServiceScope CreateScope()
        {
            if (scope == null)
                scope = this.serviceProvider.CreateScope();
            return scope;
        }
        //TODO 每次Run结束释放资源
        public void Dispose()
        {
            //TODO 持久化Scope，单个BrunContext实例共享
            if (scope != null)
            {
                scope.Dispose();
            }
            throw new NotImplementedException();
        }
    }
}
