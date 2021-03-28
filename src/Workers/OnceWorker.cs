using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Observers;
using Brun.Options;
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
    /// 基础Worker，每次执行一次
    /// </summary>
    public class OnceWorker : AbstractWorker, IOnceWorker
    {
        /// <summary>
        /// backRun实例
        /// </summary>
        private ConcurrentDictionary<Type, IBackRun> backRuns;

        //单个实例锁，只需要管自己实例
        //private readonly object backRun_LOCK = new object();
        public OnceWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            backRuns = new ConcurrentDictionary<Type, IBackRun>();
        }

        protected override async Task Brun(BrunContext context)
        {
            if (backRuns.TryGetValue(context.BrunType, out IBackRun backRun))
            {
                await backRun.Run(tokenSource.Token);
            }
            else
            {
                backRuns[context.BrunType] = (IBackRun)BrunTool.CreateInstance(context.BrunType);
                //backRuns[context.BrunType].Data = data;
                await backRuns[context.BrunType].Run(tokenSource.Token);
            }
        }

        /// <summary>
        /// BackRun最终执行
        /// </summary>
        /// <param name="brunType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected async Task Execute(Type brunType, ConcurrentDictionary<string, string> data)
        {
            //TODO 测试多线程安全
            if (backRuns.TryGetValue(brunType, out IBackRun backRun))
            {
                await backRun.Run(tokenSource.Token);
            }
            else
            {
                backRuns[brunType] = (IBackRun)BrunTool.CreateInstance(brunType);
                backRuns[brunType].Data = data;
                await backRuns[brunType].Run(tokenSource.Token);
            }
        }
        /// <summary>
        /// 直接运行不等待
        /// </summary>
        public void RunDontWait()
        {
            _ = Run();
            //TaskFactory.StartNew(() =>
            //{
            //    Run();
            //});
        }
        /// <summary>
        /// 直接运行不等待
        /// </summary>
        /// <typeparam name="TBackRun">其它的BackRun</typeparam>
        public void RunDontWait<TBackRun>()
        {
            _ = Run<TBackRun>();
            //TaskFactory.StartNew(() =>
            //{
            //    Run<TBackRun>();
            //});
        }
        /// <summary>
        /// 慎用，可能会等待后台任务
        /// </summary>
        /// <returns></returns>
        public Task Run()
        {
            return Run(_option.DefaultBrunType);
        }
        /// <summary>
        /// 慎用，可能会等待后台任务
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <returns></returns>
        public Task Run<TBackRun>()
        {
            return Run(typeof(TBackRun));
        }
        /// <summary>
        /// 运行指定类型的BanRun
        /// </summary>
        /// <returns></returns>
        public virtual Task Run(Type backRunType)
        {
            //TODO 控制并行数量

            _ = taskFactory.StartNew(async () =>
             {
                 await RealRun(backRunType);
             });
            return Task.CompletedTask;
        }
        /// <summary>
        /// 异步执行
        /// </summary>
        /// <returns></returns>
        protected virtual async Task RealRun(Type brunType)
        {
            await Observe(brunType, WorkerEvents.StartRun);
            Task task;
            try
            {
                task = Execute(brunType, _context.Items);
                RunningTasks.Add(task);
                await task;
            }
            catch (Exception ex)
            {
                _context.ExceptFromRun(ex);
                await Observe(brunType, WorkerEvents.Except);
            }
            finally
            {
                RunningTasks.TryTake(out task);
                await Observe(brunType, WorkerEvents.EndRun);
            }
        }
        public ConcurrentDictionary<string, string> GetData()
        {
            return _context.Items;
        }
        public T GetData<T>(string key)
        {
            //TODO 需要配置传入默认序列化器
            throw new NotImplementedException();
            //var r = GetData(key);
            //if (r == null)
            //    return default;
            //return (T)r;
        }
    }
}
