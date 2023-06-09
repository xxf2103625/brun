﻿using Brun.BaskRuns;
using Brun.Enums;
using Brun.Workers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 工作中心，每个实例会常驻内存
    /// </summary>
    public interface IWorker : IDisposable
    {
        /// <summary>
        /// Worker唯一Id
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Worker名称，配置时指定，不指定为类型名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Worker上下文
        /// </summary>
        WorkerContext Context { get; }
        /// <summary>
        /// Worker运行状态
        /// </summary>
        WorkerState State { get; }
        ConcurrentDictionary<string, IBackRun> BackRuns { get; }
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        void Start();
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        void Stop();
    }
}
