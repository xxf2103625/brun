﻿using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public static class WorkerServerOptionExtensions
    {
        public static WorkerServerOption UseInMemory(this WorkerServerOption workerServerOption)
        {
            workerServerOption.StoreType = WorkerStoreType.Memory;
            workerServerOption.ServicesConfigure = services =>
            {
                //TODO 初始化加载持久化内的程序集
                Brun.Commons.BrunTool.LoadFile("BrunTestHelper.dll");
                services.AddScoped<IWorkerService, WorkerService>();
                services.AddScoped<IOnceWorkerService, OnceWorkerService>();
            };
            return workerServerOption;
        }
    }
}
