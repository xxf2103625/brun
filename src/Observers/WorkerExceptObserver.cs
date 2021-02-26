using Brun.Commons;
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
        public WorkerExceptObserver() : base(WorkerEvents.Except, 10)
        {
            
        }
        public override Task Todo(WorkerContext _context)
        {
            logger = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<WorkerExceptObserver>>();
            _context.exceptNb++;
            logger.LogError("backRun:{0} is except!msg:{1}", _context.Option.BrunType, _context.Exceptions.LastOrDefault()?.Message);
            return Task.CompletedTask;
        }
    }
}
