﻿using Brun.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Observers
{
    public abstract class WorkerObserver
    {
        public WorkerObserver(WorkerEvents workerEvent, int order = 100)
        {
            this.evt = workerEvent;
            Order = order;
        }
        public WorkerEvents evt { get; }
        public int Order { get; }
        public abstract Task Todo(WorkerContext _context);
    }
}