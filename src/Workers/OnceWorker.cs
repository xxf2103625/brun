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
        protected IBackRun backRun;

        //单个实例锁，只需要管自己实例
        private readonly object backRun_LOCK = new object();
        public OnceWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
        }
        /// <summary>
        /// BackRun最终执行
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected Task Execute(ConcurrentDictionary<string, string> data)
        {
            if (backRun == null)
            {
                lock (backRun_LOCK)
                {
                    if (backRun == null)
                    {
                        backRun = (IBackRun)BrunTool.CreateInstance(_option.BrunType);
                        backRun.Data = data;

                    }
                }
            }
            return backRun.Run(tokenSource.Token);
        }
        /// <summary>
        /// 调用线程不用等待结果
        /// </summary>
        public void RunDontWait()
        {
            //_ = Run();
            Task t= Run();
            Tasks.Add(t);
        }
        /// <summary>
        /// OnceWorker执行入口
        /// </summary>
        /// <returns></returns>
        public virtual async Task Run()
        {
            await Observe(WorkerEvents.StartRun);
            try
            {
                await Execute(_context.Items);
            }catch(Exception ex)
            {
                _context.ExceptFromRun(ex);
                await Observe(WorkerEvents.Except);
            }
            finally
            {
                await Observe(WorkerEvents.EndRun);
            }
        }
        public ConcurrentDictionary<string, string> GetData()
        {
            return _context.Items;
        }
        public T GetData<T>(string key)
        {
            throw new NotImplementedException();
            //var r = GetData(key);
            //if (r == null)
            //    return default;
            //return (T)r;
        }
    }
}
