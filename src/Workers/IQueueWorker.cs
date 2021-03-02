﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public interface IQueueWorker : IWorker
    {
        Task Enqueue(string message);
        Task Start();
        void Start(object token);
    }
}