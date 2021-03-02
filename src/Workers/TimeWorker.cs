using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
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
        TimeSpan cycle;
        /// <summary>
        /// 执行次数，-1无限 
        /// </summary>
        int runNb;
        private DateTimeOffset? nextRunTime;
        private static object next_LOCK = new object();
        public TimeWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            cycle = TimeSpan.FromSeconds(5);
        }

        public async Task Start()
        {
            _context.State = Enums.WorkerState.Excuting;//worker状态
            var log = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<TimeWorker>>();
            while (!WorkerServer.Instance.StoppingToken.IsCancellationRequested)
            {
                if (nextRunTime == null)
                {
                    lock (next_LOCK)
                    {
                        if (nextRunTime == null)
                            nextRunTime = DateTime.UtcNow + cycle;
                    }
                }

                if (nextRunTime.Value <= DateTime.UtcNow)
                {
                    await Observe(Enums.WorkerEvents.StartRun);
                    Task task = WorkerServer.Instance.TaskFactory.StartNew(() =>
                    {
                        //只等待Task创建结果
                        Run();
                    }).ContinueWith(async t =>
                    {
                        lock (next_LOCK)
                        {
                            nextRunTime = DateTime.UtcNow + cycle;
                        }
                        switch (t.Status)
                        {
                            case TaskStatus.RanToCompletion:
                                //await Observe(Enums.WorkerEvents.EndRun);
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
        }
        public void Run()
        {
            var log = WorkerServer.Instance.ServiceProvider.GetRequiredService<ILogger<TimeWorker>>();
            log.LogInformation($"{DateTime.Now} - timeWorker is Run");
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
