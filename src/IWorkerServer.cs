﻿using System;
using System.Collections.Generic;

namespace Brun
{
    public interface IWorkerServer
    {
        IList<IWorker> Worders { get; }
        IWorker GetWorker(string key);
        IEnumerable<IWorker> GetWokerByName(string name);
        IQueueWorker GetQueueWorker(string key);
        IOnceWorker GetOnceWorker(string key);
        DateTime? StartTime { get; }
    }
}