using Brun.Contexts;
using Brun.Enums;
using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Observers
{
    /// <summary>
    /// 内存模式BackRun运行异常拦截器
    /// </summary>
    public class WorkerExceptRunInMemoryObserver : WorkerObserver
    {
        public WorkerExceptRunInMemoryObserver() : base(WorkerEvents.Except, 20)
        {
        }

        public override Task Todo(BrunContext brunContext)
        {
            var backRunObserverService = brunContext.ServiceProvider.GetRequiredService<IBackRunObserverService>();
            return backRunObserverService.Except(brunContext);
        }
    }
}
