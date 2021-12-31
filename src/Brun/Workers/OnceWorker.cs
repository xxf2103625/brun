using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Exceptions;
using Brun.Models;
using Brun.Options;
using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 瞬时任务执行器，调用一次执行一次
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
                base.taskFactory.StartNew(() => Execute(new BrunContext(backRun)));
                _logger.LogInformation($"OnceWorker with key '{this.Key}' is executing,backrun name:'{backRun.Name}',id:'{id}'.");
            }
            else
            {
                throw new BrunException(BrunErrorCode.NotFoundKey, $"OnceWorker can not find backrun by id '{id}'");
                //_logger.LogError("OnceWorker can not find backrun by id '{0}'", id);
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

        public void Run<TBackRun>() where TBackRun : OnceBackRun
        {
            Run(typeof(TBackRun));
        }
        public void Run(Type backRunType)
        {
            foreach (var item in _backRuns.Values.Where(m => m.GetType() == backRunType))
            {
                Run(item.Id);
            }
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
        /// //TODO 迁移到基类
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
        /// //TODO 迁移到基类
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
        public async Task<IOnceWorker> AddBrun(Type backRunType, OnceBackRunOption option = null)
        {
            using (var scope = _context.ServiceProvider.CreateScope())
            {
                var onceBrunService = scope.ServiceProvider.GetRequiredService<IOnceBrunService>();
                return await onceBrunService.AddOnceBrun(this, backRunType, option);
            }
        }
        /// <summary>
        /// 添加Brun实现类
        /// </summary>
        /// <typeparam name="TBackRun"></typeparam>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task<IOnceWorker> AddBrun<TBackRun>(OnceBackRunOption option = null) where TBackRun : OnceBackRun
        {
            return await this.AddBrun(typeof(TBackRun), option);
        }
        /// <summary>
        /// 保护内存操作，不对外开放
        /// </summary>
        /// <param name="backRunType"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        /// <exception cref="BrunException"></exception>
        internal IOnceWorker ProtectAddBrun(Type backRunType, OnceBackRunOption option = null)
        {
            if (backRunType == null)
                throw new BrunException(BrunErrorCode.ObjectIsNull, "backRunType can not be null.");
            if (option == null)
                option = new OnceBackRunOption();
            if (!backRunType.IsSubclassOf(typeof(OnceBackRun)))
            {
                throw new BrunException(BrunErrorCode.TypeError, $"{backRunType.FullName} can not add to OnceWorker,must be SubclassOf OnceBackRun");
            }
            if (!string.IsNullOrEmpty(option.Id) && _backRuns.ContainsKey(option.Id))
            {
                throw new BrunException(BrunErrorCode.AllreadyKey, $"the OnceWorker with key:'{this.Key}' has allready contains BackRun type:'{backRunType.FullName}'.");
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
                    return this;
                }
                else
                {
                    throw new BrunException(BrunErrorCode.TypeError, string.Format("can not add {0} to OnceWorker.", backRunType.FullName));
                }
            }
        }
    }
}
