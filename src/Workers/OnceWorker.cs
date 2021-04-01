using Brun.BaskRuns;
using Brun.Commons;
using Brun.Contexts;
using Brun.Options;
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
        /// <summary>
        /// 包含的Backrun
        /// </summary>
        private ConcurrentDictionary<Type, IBackRun> backRuns;

        public override IEnumerable<Type> BrunTypes => backRuns.Keys;

        public OnceWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
            backRuns = new ConcurrentDictionary<Type, IBackRun>();
        }
        /// <summary>
        /// 启动线程，开始执行Execute
        /// </summary>
        public virtual Task StartBrun(Type brunType)
        {
            BrunContext brunContext = new BrunContext(brunType);
            return taskFactory.StartNew(() =>
            {
                _ = Execute(brunContext);
            });
        }
        protected override async Task Brun(BrunContext context)
        {
            if (backRuns.TryGetValue(context.BrunType, out IBackRun backRun))
            {
                await backRun.Run(tokenSource.Token);
            }
            else
            {
                backRuns[context.BrunType] = (IBackRun)BrunTool.CreateInstance(context.BrunType);
                backRuns[context.BrunType].Data = _context.Items;
                await backRuns[context.BrunType].Run(tokenSource.Token);
            }
        }
        public ConcurrentDictionary<string, string> GetData()
        {
            return _context.Items;
        }
        public void Run()
        {
            Run(_option.DefaultBrunType);
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
    }
}
