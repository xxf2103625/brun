using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Brun.Enums;
using Microsoft.Extensions.Logging;

namespace Brun
{
    public sealed class WorkerContext : IDisposable
    {
        //private ILogger<WorkerContext> _logger;
        private WorkerOption _option;
        private WorkerConfig _config;
        private IServiceProvider serviceProvider;
        //元数据
        private IDictionary<string, object> meta;
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
        public WorkerContext(WorkerOption workerOption, WorkerConfig config)
        {
            _option = workerOption;
            _config = config;
            items = _option.Data;
            Init();
        }
        private void Init()
        {
            serviceProvider = WorkerServer.Instance.ServiceProvider;
        }
        /// <summary>
        /// Worker唯一标识
        /// </summary>
        public string Key => _option.Key;
        /// <summary>
        /// Worker名称，默认类型名称
        /// </summary>
        public string Name => _option.Name;
        /// <summary>
        /// 自定义Tag
        /// </summary>
        public string Tag => _option.Tag;
        public WorkerOption Option => _option;
        /// <summary>
        /// BackRun运行异常列表，默认最多储存10个
        /// </summary>
        public IList<Exception> Exceptions => exceptions;
        //public IServiceProvider ServiceProvider => serviceProvider;
        /// <summary>
        /// 添加run异常事件
        /// </summary>
        /// <param name="ex"></param>
        public void ExceptFromRun(Exception ex)
        {
            if (exceptions == null)
                exceptions = new List<Exception>();
            if (exceptions.Count > _config.WorkerContextMaxExcept)
                exceptions.RemoveAt(0);
            exceptions.Add(ex);
        }
        /// <summary>
        /// 当前状态
        /// </summary>
        public WorkerState State { get; set; }
        public void Dispose()
        {
            items?.Clear();
            exceptions?.Clear();
        }
        public ConcurrentDictionary<string, string> Items => items;
    }
}