using Brun.BaskRuns;
using Brun.Commons;
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
            //_ = Run();
            TaskFactory.StartNew(() =>
            {
                Run();
            });
        }
        /// <summary>
        /// 直接运行不等待
        /// </summary>
        /// <typeparam name="TBackRun">其它的BackRun</typeparam>
        public void RunDontWait<TBackRun>()
        {
            TaskFactory.StartNew(() =>
            {
                Run<TBackRun>();
            });
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
        private object Task_LOCK = new object();
        /// <summary>
        /// 运行指定类型的BanRun
        /// </summary>
        /// <returns></returns>
        public async Task Run(Type backRunType)
        {
            //TODO 控制并行数量
            //return RealRun(backRunType);
            //if (RunningTasks.Count > 4)
            //{
            //    Thread.Sleep(5);
            //}

            Task task =await taskFactory.StartNew(async () =>
            {
                await RealRun(backRunType);
            });

            RunningTasks.Add(task);

            _ = task.ContinueWith(t =>
            {
                RunningTasks.TryTake(out t);

            });
            //RunningTasks.Add(TaskFactory.StartNew(() =>
            //{
            //    Task t = RealRun(backRunType);
            //    t.ContinueWith(t =>
            //    {
            //        RunningTasks.TryTake(out t);
            //    });
            //    t.Start();
            //}));

            //RunningTasks.TryAdd(t);

            //return t;
            //return Task.CompletedTask;
        }
        /// <summary>
        /// 异步执行
        /// </summary>
        /// <returns></returns>
        private async Task RealRun(Type brunType)
        {
            //await Observe(brunType, WorkerEvents.StartRun);
            //try
            //{
            //    await Execute(brunType, _context.Items);
            //}
            //catch (Exception ex)
            //{
            //    _context.ExceptFromRun(ex);
            //    await Observe(brunType, WorkerEvents.Except);
            //}
            //finally
            //{
            //    await Observe(brunType, WorkerEvents.EndRun);
            //}
            Task start = Observe(brunType, WorkerEvents.StartRun);
            Task eTask = await start.ContinueWith(async t =>
             {
                 try
                 {
                     await Execute(brunType, _context.Items);
                 }
                 catch (Exception ex)
                 {
                     _context.ExceptFromRun(ex);
                     await Observe(brunType, WorkerEvents.Except);
                 }
             });
            Task endTask = await eTask.ContinueWith(async t =>
             {
                 await Observe(brunType, WorkerEvents.EndRun);
             });
            await endTask;
            //await (await Observe(brunType, WorkerEvents.StartRun).ContinueWith(async t =>
            //{
            //    try
            //    {
            //        await Execute(brunType, _context.Items);
            //    }
            //    catch (Exception ex)
            //    {
            //        _context.ExceptFromRun(ex);
            //        await Observe(brunType, WorkerEvents.Except);
            //    }
            //})).ContinueWith(async t =>
            //{
            //    await Observe(brunType, WorkerEvents.EndRun);
            //});
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
