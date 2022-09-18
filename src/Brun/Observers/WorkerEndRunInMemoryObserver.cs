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
    /// 内存模式BackRun每次运行结束的拦截器
    /// </summary>
    public class WorkerEndRunInMemoryObserver : WorkerObserver
    {
        public WorkerEndRunInMemoryObserver() : base(WorkerEvents.EndRun, 20)
        {
        }

        public override Task Todo(BrunContext brunContext)
        {
            var backRunObserverService = brunContext.ServiceProvider.GetRequiredService<IBackRunObserverService>();
            return backRunObserverService.End(brunContext);
        }
    }
}
