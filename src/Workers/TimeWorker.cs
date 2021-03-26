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
    /// 简单的时间循环任务，复杂的定时使用<see cref="PlanTimeWorker"/>
    /// //TODO 最简易循环执行任务，继续简化使用
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
        //只需要实例内的锁
        private object next_LOCK = new object();
        private object backRun_LOCK = new object();
        private IBackRun _backRun;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <param name="config"></param>
        public TimeWorker(TimeWorkerOption option, WorkerConfig config) : base(option, config)
        {
            if (option.Cycle == TimeSpan.Zero)
                throw new Exception("TimeWorker Cycle not set or can't be Zero");
            cycle = option.Cycle;
            if (option.RunWithStart)
            {
                nextRunTime = DateTime.Now;
            }
        }
        /// <summary>
        /// 实例内保持唯一
        /// </summary>
        protected IBackRun BackRun
        {
            get
            {
                if (_backRun == null)
                {
                    lock (backRun_LOCK)
                    {
                        if (_backRun == null)
                        {
                            _backRun = (IBackRun)BrunTool.CreateInstance(_option.DefaultBrunType);
                            _backRun.Data = _context.Items;
                        }

                    }
                }
                return _backRun;
            }
        }
        /// <summary>
        /// 启动Worker
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            _context.State = Enums.WorkerState.Started;//worker状态
            var log = _context.ServiceProvider.GetRequiredService<ILogger<TimeWorker>>();
            //TODO 优化定时流程
            while (!tokenSource.Token.IsCancellationRequested)
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

                    Task task = Execute();
                    _ = RunningTasks.TryAdd(task);
                    _ = task.ContinueWith(t =>
                      {
                          _ = RunningTasks.TryTake(out t);

                      });
                    lock (next_LOCK)
                    {
                        nextRunTime = DateTime.Now + cycle;
                    }
                }
                await Task.Delay(5);
            }
            _context.State = Enums.WorkerState.Stoped;//worker状态
        }
        /// <summary>
        /// 执行一次队列任务
        /// </summary>
        /// <returns></returns>
        protected async Task Execute()
        {
            Type brunType = _option.DefaultBrunType;
            Task start = Observe(brunType, Enums.WorkerEvents.StartRun);
            await start.ContinueWith(async t =>
              {
                  try
                  {
                      await BackRun.Run(tokenSource.Token);
                  }
                  catch (Exception ex)
                  {
                      _context.ExceptFromRun(ex);
                      await Observe(brunType, Enums.WorkerEvents.Except);
                  }
                  finally
                  {
                      await Observe(brunType, Enums.WorkerEvents.EndRun);
                  }
              });

        }
        //TODO TimeWorker暂停
        public Task Pause()
        {
            throw new NotImplementedException();
        }
        //TODO TimeWorker恢复
        public Task Resume()
        {
            throw new NotImplementedException();
        }


    }
}
