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

        public override Task Todo(WorkerContext _context, Type brunType)
        {
            logger = _context.ServiceProvider.GetRequiredService<ILogger<WorkerExceptObserver>>();
            _context.exceptNb++;
            logger.LogError("{4} backRun:{0} is except! error count:{1},msg:{2},StackTrace:{3}", brunType, _context.Exceptions?.Count, _context.Exceptions.LastOrDefault()?.Message, _context.Exceptions.LastOrDefault()?.StackTrace, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return Task.CompletedTask;
        }
    }
}
