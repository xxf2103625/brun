using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Exceptions;
using Brun.Models;
using Brun.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 基础Worker，每次执行一次
    /// </summary>
    public class OnceWorker : AbstractWorker, IOnceWorker
    {
        private IBackRun defuatBackRun = null;
        public OnceWorker(WorkerConfig config) : base(config)
        {
            Init();
        }

        protected virtual void Init()
        {
            if (string.IsNullOrEmpty(this._config.Key))
            {
                this._config.Key = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(this._config.Name))
            {
                this._config.Name = nameof(OnceWorker);
            }
            _logger.LogInformation($"OnceWorker with key '{this.Key}' is init.");
        }
        /// <summary>
        /// 内存对象start，对外隐藏
        /// </summary>
        internal override void ProtectStart()
        {
            _context.State = WorkerState.Started;
            _logger.LogInformation("the {0} key:'{1}' is started", GetType().Name, _context.Key);
        }

        public void Run(string id)
        {
            if (_backRuns.TryGetValue(id, out IBackRun backRun))
            {
                //TODO 危险代码，BackRun返回async/Task.CompletedTask 执行流程不同,可能会等待
                //_ = Execute(new BrunContext(backRun));

                base.taskFactory.StartNew(() => Execute(new BrunContext(backRun)));

                _logger.LogInformation($"OnceWorker with key '{this.Key}' is executing,backrun name:'{backRun.Name}',id:'{id}'.");
            }
            else
            {
                _logger.LogError("OnceWorker can not find backrun by id '{0}'", id);
            }
        }
        /// <summary> 
        /// 开始执行Execute //TODO 以id为基础
        /// </summary>
        public virtual void StartBrun(Type brunType)
        {
            foreach (var item in _backRuns.Values.Where(m => m.GetType() == brunType))
            {
                Run(item.Id);
            }
        }
        protected override async Task Brun(BrunContext context)
        {
            await ((OnceBackRun)context.BackRun).Run(tokenSource.Token);
        }
        public ConcurrentDictionary<string, string> GetData()
        {
            return _context.Items;
        }
        /// <summary>
        /// 运行默认第一个添加的BackRun
        /// </summary>
        public void Run()
        {
            if (defuatBackRun != null)
            {
                Run(defuatBackRun.Id);
            }
            else
            {
                _logger.LogError("the OnceWorker with key:'{0}' don't have defaultBackRun.", this.Key);
            }
        }

        public void Run<TBackRun>()
        {
            Run(typeof(TBackRun));
        }
        public void Run(Type backRunType)
        {
            StartBrun(backRunType);
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
        /// <summary>
        /// 设置共享数据(会覆盖)
        /// </summary>
        /// <param name="sharedData"></param>
        /// <returns></returns>
        public OnceWorker SetData(ConcurrentDictionary<string, string> sharedData)
        {
            this._context.Items = sharedData;
            return this;
        }
        /// <summary>
        /// 添加共享数据(已有不会覆盖)
        /// </summary>
        /// <param name="sharedData"></param>
        /// <returns></returns>
        public OnceWorker AddData(ConcurrentDictionary<string, string> sharedData)
        {
            if (this.Context.Items == null)
            {
                this.Context.Items = sharedData;
            }
            else
            {
                _logger.LogWarning($"the OnceWorker with key:'{this.Key}' allready has shared data,you can try SetData().");
            }
            return this;
        }
        /// <summary>
        /// 添加Brun实现类
        /// </summary>
        /// <param name="backRunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <exception cref="BrunException"></exception>
        public OnceBackRun AddBrun(Type backRunType, OnceBackRunOption option)
        {
            if (backRunType == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "backRunType can not be null.");
            if (option == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "BackRunOption can not be null.");
            //_logger.LogDebug($"the OnceWorker with key:'{this.Key}' is adding '{backRunType.FullName}'");
            if (!backRunType.IsSubclassOf(typeof(BackRun)))
            {
                throw new BrunException(BrunErrorCode.TypeError, $"{backRunType.FullName} can not add to OnceWorker.");
            }
            if (!string.IsNullOrEmpty(option.Id) && _backRuns.ContainsKey(option.Id))
            {
                //已包含
                //throw new BrunException(BrunErrorCode.AllreadyKey, $"the OnceWorker with key:'{this.Key}' has allready contains BackRun type:'{backRunType.FullName}'.");

                _logger.LogWarning($"the OnceWorker with key:'{this.Key}' has allready contains BackRun type:'{backRunType.FullName}'.");
                throw new BrunException(BrunErrorCode.AllreadyKey, "the once backRun key is allready in system");
            }
            else
            {
                if (option.Id == null)
                    option.Id = Guid.NewGuid().ToString();
                if (option.Name == null)
                    option.Name = backRunType.Name;
                OnceBackRun brun = (OnceBackRun)(IBackRun)BrunTool.CreateInstance(backRunType, option);
                brun.SetWorkerContext(this._context);
                if (_backRuns.TryAdd(brun.Id, brun))
                {
                    if (_backRuns.Count == 1)
                    {
                        this.defuatBackRun = brun;
                    }
                    _logger.LogInformation("the OnceWorker with key:'{0}' added BackRun:'{1}'.", this.Key, backRunType.FullName);
                    return brun;
                }
                else
                {
                    throw new BrunException(BrunErrorCode.TypeError, string.Format("can not add {0} to OnceWorker.", backRunType.FullName));
                }
            }
        }
        /// <summary>
        /// 添加Brun实现类
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public OnceBackRun AddBrun<TBackRun>(OnceBackRunOption option) where TBackRun : OnceBackRun
        {
            return this.AddBrun(typeof(TBackRun), option);
        }
        public OnceBackRun AddBrun<TBackRun>() where TBackRun : OnceBackRun
        {
            return this.AddBrun(typeof(TBackRun), new OnceBackRunOption());
        }
    }
}
