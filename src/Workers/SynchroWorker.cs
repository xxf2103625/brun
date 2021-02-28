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
    public class SynchroWorker : OnceWorker
    {

        public SynchroWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            config.AddWorkerObserver(new SynchroBeforRunObserver());
        }
        //protected Task Execute(ConcurrentDictionary<string, string> data)
        //{

        //    IBackRun backRun = (BackRun)BrunTool.CreateInstance(_option.BrunType);
        //    backRun.Data = data;
        //    return backRun.Run(WorkerServer.Instance.StoppingToken);
        //}
        //public override async Task Run()
        //{
        //    await Observe(WorkerEvents.StartRun);
        //    try
        //    {
        //        await Execute(_context.Items);
        //    }
        //    catch (Exception ex)
        //    {
        //        _context.ExceptFromRun(ex);
        //        await Observe(WorkerEvents.Except);
        //    }
        //    finally
        //    {
        //        await Observe(WorkerEvents.EndRun);
        //    }
        //}

        //public void RunDontWait()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
