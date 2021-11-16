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
        //private ILogger<WorkerContext> _logger;
        //private WorkerOption _option;
        private WorkerConfig _config;
        //元数据，用于后期持久化
        //private IDictionary<string, object> meta;
        //BackRun自定义的数据
        private ConcurrentDictionary<string, string> items;
        //异常堆栈
        private IList<Exception> exceptions;
        //BackRun开始运行计数
        public long startNb = 0;
        //BackRun结束运行计数，异常也算结束
        public long endNb = 0;
        //BackRun运行异常计数
        public int exceptNb = 0;

        public WorkerContext(WorkerConfig config)
        {
            //_option = workerOption;
            _config = config;
            Init();
        }
        private void Init()
        {
            //if (_option.Data == null)
            //{
            //    items = new ConcurrentDictionary<string, string>();
            //}
            //else
            //{
            //    items = _option.Data;
            //}
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
        /// 自定义Tag
        /// </summary>
        public string Tag => _config.Tag;
        ///// <summary>
        ///// 配置
        ///// </summary>
        //public WorkerOption Option => _option;
        /// <summary>
        /// BackRun运行异常列表，默认最多储存10个
        /// </summary>
        public IList<Exception> Exceptions => exceptions;
        /// <summary>
        /// 添加run异常事件
        /// </summary>
        /// <param name="ex"></param>
        public void ExceptFromRun(Exception ex)
        {
            if (exceptions == null)
                exceptions = new List<Exception>();
            if (exceptions.Count >= _config.WorkerContextMaxExcept)
                exceptions.RemoveAt(0);
            exceptions.Add(ex);
        }
        /// <summary>
        /// Worker状态
        /// </summary>
        public WorkerState State { get; set; }
        /// <summary>
        /// OnceWorker共享数据
        /// </summary>
        public ConcurrentDictionary<string, string> Items { get => items; set { items = value; } }
        public IServiceProvider ServiceProvider => WorkerServer.Instance.ServiceProvider;
        public BlockingCollection<Task> Tasks { get; set; }
        public void Dispose()
        {
            items?.Clear();
            exceptions?.Clear();
        }
    }
}