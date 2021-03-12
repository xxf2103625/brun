using Brun.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Observers
{
    public class QueueListenObserver<TMessage> : WorkerObserver
    {
        public QueueListenObserver() : base(WorkerEvents.StartRun, 5)
        {

        }

        public override Task Todo(WorkerContext _context, Type brunType)
        {
            throw new NotImplementedException();
        }
    }
}
