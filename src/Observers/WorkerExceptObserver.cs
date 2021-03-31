using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Observers
{
    public class WorkerExceptObserver : WorkerObserver
    {
        private ILogger<WorkerExceptObserver> logger;
        //private static object nb_LOCK = new object();
        public WorkerExceptObserver() : base(WorkerEvents.Except, 10)
        {

        }

        public override Task Todo(WorkerContext _context, BrunContext brunContext)
        {
            logger = _context.ServiceProvider.GetRequiredService<ILogger<WorkerExceptObserver>>();
            System.Threading.Interlocked.Increment(ref _context.exceptNb);
            //lock (Observer_LOCK)
            //{
            //    _context.exceptNb++;
            //}
            //brunContext.ExceptNb = _context.exceptNb;
            logger.LogError(" backRun:{0} is except!count:{1},msg:{2},StackTrace:{3} {4}", brunContext.BrunType.Name, _context.exceptNb, _context.Exceptions.LastOrDefault()?.Message, _context.Exceptions.LastOrDefault()?.StackTrace, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return Task.CompletedTask;
        }
    }
}
