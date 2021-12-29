using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brun.Enums;
using Brun.Options;
using Microsoft.Extensions.Logging;

namespace Brun
{
    public sealed class WorkerContext : IDisposable
    {
        private WorkerConfig _config;
        //元数据，用于后期持久化
        //private IDictionary<string, object> meta;

        /// <summary>
        /// BackRun开始运行计数
        /// </summary>
        public long startNb = 0;
        /// <summary>
        /// BackRun结束运行计数，异常也算结束
        /// </summary>
        public long endNb = 0;
        /// <summary>
        /// BackRun运行异常计数
        /// </summary>
        public int exceptNb = 0;

        public WorkerContext(WorkerConfig config)
        {
            _config = config;
            Init();
        }
        private void Init()
        {
            this.Items = new ConcurrentDictionary<string, string>();
        }
        /// <summary>
        /// Worker唯一标识
        /// </summary>
        public string Key => _config.Key;
        /// <summary>
        /// Worker名称，默认类型名称
        /// </summary>
        public string Name => _config.Name;
        /// <summary>
        /// Worker状态
        /// </summary>
        public WorkerState State { get; set; }
        /// <summary>
        /// OnceWorker共享数据
        /// </summary>
        public ConcurrentDictionary<string, string> Items { get; set; }
        public IServiceProvider ServiceProvider => WorkerServer.Instance.ServiceProvider;
        public ILoggerFactory LoggerFactory => WorkerServer.Instance.LoggerFactory;
        /// <summary>
        /// 当前Worker中正在运行BackRun的Task集合
        /// </summary>
        public BlockingCollection<Task> RunningTasks { get; set; }
        public void Dispose()
        {
            Items?.Clear();
        }
    }
}