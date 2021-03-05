using Brun.BaskRuns;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 包含容器和自定义数据的单次后台任务
    /// </summary>
    public abstract class BackRun : BackRunServicePrivoder, IBackRun
    {
        /// <summary>
        /// 每次运行共享的自定义数据
        /// </summary>
        public ConcurrentDictionary<string, string> Data { get; set; }
        /// <summary>
        /// 定义长时间任务时，自己用stoppingToken控制任务尽快结束
        /// </summary>
        /// <param name="stoppingToken">进程结束信号</param>
        /// <returns></returns>
        public abstract Task Run(CancellationToken stoppingToken);

    }
}
