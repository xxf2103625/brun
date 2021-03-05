using Brun.BaskRuns;
using Brun.Commons;
using Brun.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 简单的内存定时任务,这种执行周期会偏移
    /// </summary>
    public class TimeWorker : AbstractWorker, ITimeWorker
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTimeOffset? beginTime;
        /// <summary>
        /// 周期
        /// </summary>
        private TimeSpan cycle;
        /// <summary>
        /// 执行次数，-1无限 
        /// </summary>
        private int runNb;
        private DateTimeOffset? nextRunTime;
        private static object next_LOCK = new object();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="config"></param>
        public TimeWorker(TimeWorkerOption option, WorkerConfig config) : base(option, config)
        {
            if (option.Cycle == TimeSpan.Zero)
                throw new Exception("TimeWorker Cycle not set or do not set Zero");
            cycle = option.Cycle;
            if (option.RunWithStart)
            {
                nextRunTime = DateTime.Now;
            }
        }
        /// <summary>
        /// 启动Worker
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            _context.State = Enums.WorkerState.Excuting;//worker状态
            var log = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<TimeWorker>>();
            while (!WorkerServer.Instance.StoppingToken.IsCancellationRequested)
            {
                if (nextRunTime == null || nextRunTime.Value <= DateTime.Now)
                {
                    lock (next_LOCK)
                    {
                        if (nextRunTime == null)
                        {
                            nextRunTime = DateTime.Now + cycle;
                            continue;
                        }
                        else
                        {
                            if (nextRunTime.Value > DateTime.Now)
                                continue;
                        }
                    }
                }
                if (nextRunTime.Value <= DateTime.Now)
                {
                    await Observe(Enums.WorkerEvents.StartRun);
                    Task task = WorkerServer.Instance.TaskFactory.StartNew(async () =>
                    {
                        //只等待Task创建结果
                        await Execute(_context.Items);
                    }).ContinueWith(async t =>
                    {
                        lock (next_LOCK)
                        {
                            nextRunTime = DateTime.Now + cycle;
                        }
                        switch (t.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                //await Observe(Enums.WorkerEvents.EndRun);
                                switch (t.Result.Status)
                                {
                                    case TaskStatus.RanToCompletion:
                                        //正常结束
                                        break;
                                    case TaskStatus.Faulted:
                                        //task内部异常
                                        Type t1 = t.Result.Exception.GetType();
                                        Type t2 = t.Result.Exception.InnerException.GetType();
                                        _context.ExceptFromRun(t.Result.Exception?.InnerException);
                                        await Observe(Enums.WorkerEvents.Except);
                                        break;
                                    case TaskStatus.Canceled:
                                        //token取消
                                        break;
                                }
                                break;
                            case TaskStatus.Faulted:
                                _context.ExceptFromRun(new Exception("TimeWorker run TaskStatus.Faulted", t.Exception));
                                await Observe(Enums.WorkerEvents.Except);
                                break;
                            case TaskStatus.Canceled:
                                _context.ExceptFromRun(new Exception("TimeWorker run TaskStatus.Canceled", t.Exception));
                                await Observe(Enums.WorkerEvents.Except);
                                break;
                        }
                        await Observe(Enums.WorkerEvents.EndRun);
                    }, WorkerServer.Instance.StoppingToken);
                }
                await Task.Delay(5);
            }
            _context.State = Enums.WorkerState.Paused;//worker状态
        }
        public void Run()
        {
            var log = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<TimeWorker>>();
            log.LogInformation($"{DateTime.Now} - timeWorker is Run");
        }
        protected Task Execute(ConcurrentDictionary<string, string> data)
        {
            IBackRun backRun = (IBackRun)BrunTool.CreateInstance(_option.BrunType);
            backRun.Data = data;
            return backRun.Run(WorkerServer.Instance.StoppingToken);
        }
        public Task Pause()
        {
            throw new NotImplementedException();
        }

        public Task Resume()
        {
            throw new NotImplementedException();
        }


    }
}
