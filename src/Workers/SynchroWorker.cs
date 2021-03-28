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
    /// 同步Worker，同一个Worker内，backrun串行运行
    /// //TODO 优化同步Worker
    /// </summary>
    public class SynchroWorker : OnceWorker
    {
        private static object Sync_LOCK = new object();
        private bool isReady = true;
        private int maxThread = 1;
        public SynchroWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            //config.AddWorkerObserver(new SynchroBeforRunObserver());
        }
        /// <summary>
        /// 运行指定类型的BanRun
        /// </summary>
        /// <returns></returns>
        public  Task Run(Type backRunType)
        {
            //TODO 直接控制线程池
            //TODO 控制并行数量
            lock (Sync_LOCK)
            {
                while (maxThread <= 0)
                {
                    Thread.Sleep(50);
                }
                maxThread--;
            }
            _ = taskFactory.StartNew(() =>
            {
                Task task = Run(backRunType);
                task.ContinueWith(t =>
                {
                    maxThread++;
                });
            });

            return Task.CompletedTask;
        }
        ///// <summary>
        ///// 运行指定类型的BanRun
        ///// </summary>
        ///// <returns></returns>
        //public override Task Run(Type backRunType)
        //{
        //    //TODO 控制并行数量
        //    lock (Sync_LOCK)
        //    {
        //        while (!isReady)
        //        {
        //            Thread.Sleep(50);
        //        }
        //        isReady = false;
        //    }
        //    _ = taskFactory.StartNew(() =>
        //    {
        //        Task task = RealRun(backRunType);
        //        task.ContinueWith(t =>
        //        {
        //            isReady = true;
        //        });
        //    });

        //    return Task.CompletedTask;
        //}

    }
}
