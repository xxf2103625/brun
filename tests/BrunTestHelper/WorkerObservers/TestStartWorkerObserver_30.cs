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
    public class TestStartWorkerObserver_30 : WorkerObserver
    {
        public TestStartWorkerObserver_30() : base(WorkerEvents.StartRun, 30)
        {
        }

        public override async Task Todo(WorkerContext _context, BrunContext brunContext)
        {
            var log = _context.ServiceProvider.GetRequiredService<ILogger<TestStartWorkerObserver_30>>();
            log.LogInformation("TestStartWorkerObserver_30 Order:{0},Time:{1}.", this.Order, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss FFFF"));
            lock (Observer_LOCK)
            {
                _context.Items["Order"] = "30";
            }
            await Task.Delay(TimeSpan.FromSeconds(0.1));
        }
    }
}
