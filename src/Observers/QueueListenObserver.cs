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
    public class QueueListenObserver<TMessage> : WorkerObserver
    {
        public QueueListenObserver() : base(WorkerEvents.StartRun, 5)
        {

        }

        public override  Task Todo(WorkerContext _context)
        {
            //IBrunQueueService<TMessage> queueService = _context.ServiceProvider.GetRequiredService<IBrunQueueService<TMessage>>();
            //IBrunQueueService<TMessage> queueService = WorkerServer.Instance.ServiceProvider.GetService<IBrunQueueService<TMessage>>();
            //while (queueService.TryDequeue(out TMessage msg))
            //{
            //    Console.WriteLine("msg");
            //    await Task.Delay(TimeSpan.FromSeconds(1));
            //}
            return Task.CompletedTask;
        }
    }
}
