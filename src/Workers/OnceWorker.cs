using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Exceptions;
using Brun.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 基础Worker，每次执行一次
    /// </summary>
    public class OnceWorker : AbstractWorker, IOnceWorker
    {

        private Type defuatBackRun = null;
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
        /// 开始执行Execute
        /// </summary>
        public virtual async Task StartBrun(Type brunType)
        {
            if (_backRuns.TryGetValue(brunType.FullName, out IBackRun backRun))
            {
                await Execute(new BrunContext(backRun));
            }
            else
            {
                _backRuns[brunType.FullName] = (IBackRun)BrunTool.CreateInstance(brunType);
                _backRuns[brunType.FullName].SetWorkerContext(_context);
                await Execute(new BrunContext(_backRuns[brunType.FullName]));
            }
            _logger.LogInformation($"OnceWorker with key '{this.Key}' is executed.");
        }
        protected override async Task Brun(BrunContext context)
        {
            await context.BackRun.Run(tokenSource.Token);
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
                Run(defuatBackRun);
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
            Task.Run(async () =>
            {
                await StartBrun(backRunType);
            });
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
        /// <returns></returns>
        /// <exception cref="BrunTypeErrorException"></exception>
        public OnceWorker AddBrun(Type backRunType)
        {
            //_logger.LogDebug($"the OnceWorker with key:'{this.Key}' is adding '{backRunType.FullName}'");
            if (!backRunType.IsSubclassOf(typeof(BackRun)))
            {
                throw new BrunTypeErrorException($"{backRunType.FullName} can not add to OnceWorker.");
            }
            if (_backRuns.ContainsKey(backRunType.FullName))
            {
                //已包含,返回已有对象
                _logger.LogWarning($"the OnceWorker with key:'{this.Key}' has allready contains BackRun type:'{backRunType.FullName}'.");
                return this;
            }
            else
            {
                IBackRun brun = (IBackRun)BrunTool.CreateInstance(backRunType);
                brun.SetWorkerContext(this._context);
                if (_backRuns.TryAdd(backRunType.FullName, brun))
                {
                    if (_backRuns.Count == 1)
                    {
                        this.defuatBackRun = backRunType;
                    }
                    _logger.LogInformation("the OnceWorker with key:'{0}' added BackRun:'{1}'.", this.Key, backRunType.FullName);
                    return this;
                }
                else
                {
                    throw new BrunTypeErrorException(string.Format("can not add {0} to OnceWorker.", backRunType.FullName));
                }
            }
        }
        /// <summary>
        /// 添加Brun实现类
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <returns></returns>
        public OnceWorker AddBrun<TBackRun>() where TBackRun : BackRun, new()
        {
            return this.AddBrun(typeof(TBackRun));
        }
    }
}
