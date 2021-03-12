using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public interface IQueueWorker : IWorker
    {
        Task Start();
        void Enqueue(string message);
        //Task Start();
        void Enqueue<TQueueBackRun>(string message);
        void Enqueue(Type queueBackRunType, string message);
        void Enqueue(string queueBackRunTypeFullName, string message);
    }
}
