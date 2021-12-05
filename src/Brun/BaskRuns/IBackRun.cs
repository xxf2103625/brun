using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.BaskRuns
{
    /// <summary>
    /// 所有后台任务的接口,直接继承这个无法使用自定义Data
    /// </summary>
    public interface IBackRun
    {
        string Id { get; }
        string Name { get; }
        ConcurrentDictionary<string, string> Data { get; }
        WorkerContext WorkerContext { get; }

        Task Run(CancellationToken stoppingToken);
        void SetWorkerContext(WorkerContext workerContext);
    }
}
