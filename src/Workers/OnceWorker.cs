﻿using Brun.Commons;
using Brun.Enums;
using Brun.Observers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 基础Worker，每次执行一次
    /// </summary>
    public class OnceWorker : AbstractWorker
    {
        public OnceWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
        }
        protected Task Execute(ConcurrentDictionary<string, string> data)
        {
            IBackRun backRun = (IBackRun)BrunTool.CreateInstance(_option.BrunType);
            backRun.Data = data;
            return backRun.Run(WorkerServer.Instance.StoppingToken);
        }
        public override async Task Run()
        {
            await Observe(WorkerEvents.StartRun);
            try
            {
                await Execute(_context.Items);
            }
            catch (Exception ex)
            {
                _context.ExceptFromRun(ex);
                await Observe(WorkerEvents.Except);
            }
            finally
            {
                await Observe(WorkerEvents.EndRun);
            }
        }
    }
}
