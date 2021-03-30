using Brun;
using Brun.Contexts;
using Brun.Enums;
using Brun.Observers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunTestHelper.WorkerObservers
{
    public class TestStartWorkerObserver_20 : WorkerObserver
    {
        public TestStartWorkerObserver_20() : base(WorkerEvents.StartRun, 20)
        {
        }
        public override async Task Todo(WorkerContext _context, BrunContext brunContext)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            var log = _context.ServiceProvider.GetRequiredService<ILogger<TestStartWorkerObserver_20>>();
            log.LogInformation("TestStartWorkerObserver_20 Order:{0},Time:{1}.", this.Order,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss FFFF"));
            lock (Observer_LOCK)
            {
                _context.Items["Order"] = "20";
            }
            //return Task.CompletedTask;
        }
    }
}
