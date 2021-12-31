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
    /// 内存模式添加的拦截器，记录运行历史信息
    /// </summary>
    public class WorkerStartRunInMemoryObserver : WorkerObserver
    {
        public WorkerStartRunInMemoryObserver() : base(WorkerEvents.StartRun, 20)
        {
        }

        public override Task Todo(BrunContext brunContext)
        {
            var backRunObserverService = brunContext.ServiceProvider.GetRequiredService<IBackRunObserverService>();
            return backRunObserverService.Start(brunContext);
        }
    }
}
