using System.Collections.Generic;

namespace Brun
{
    public interface IWorkerServer
    {
        IWorker GetWorker(string key);
        IEnumerable<IWorker> GetWokerByName(string name);
        IEnumerable<IWorker> GetWokerByTag(string tag);
        IQueueWorker GetQueueWorker(string key);
    }
}