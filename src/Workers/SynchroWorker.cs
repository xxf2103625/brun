using Brun.Commons;
using Brun.Enums;
using Brun.Observers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 同步Worker，同一个Worker内，同一时间执行backrun会强制排队运行
    /// </summary>
    public class SynchroWorker : AbstractWorker
    {

        public SynchroWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            config.AddWorkerObserver(new Observers.SynchroBeforRunObserver());
        }
        protected Task Execute(ConcurrentDictionary<string, string> data)
        {

            IBackRun backRun = (BackRun)BrunTool.CreateInstance(_option.BrunType);
            backRun.Data = data;
            return backRun.Run(WorkerServer.Instance.StoppingToken);
        }
        public override async Task Run()
        {
            IEnumerable<WorkerObserver> startRunObservers = _config.GetObservers(WorkerEvents.StartRun);
            foreach (var item in startRunObservers.OrderBy(m => m.Order))
            {
                await item.Todo(_context);
            }
            try
            {
                await Execute(_context.Items);
            }
            catch (Exception ex)
            {
                _context.ExceptFromRun(ex);
                IEnumerable<WorkerObserver> exceptRunObservers = _config.GetObservers(WorkerEvents.Except);
                foreach (var item in exceptRunObservers.OrderBy(m => m.Order))
                {
                    await item.Todo(_context);
                }
            }
            finally
            {
                IEnumerable<WorkerObserver> endRunObservers = _config.GetObservers(WorkerEvents.EndRun);
                foreach (var item in endRunObservers.OrderBy(m => m.Order))
                {
                    await item.Todo(_context);
                }
            }
        }

        
    }
}
