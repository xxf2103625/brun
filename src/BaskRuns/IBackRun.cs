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
        ConcurrentDictionary<string, string> Data { get; set; }
        Task Run(CancellationToken stoppingToken);
    }
}
