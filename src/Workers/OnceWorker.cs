using Brun.BaskRuns;
using Brun.Commons;
using Brun.Enums;
using Brun.Observers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 基础Worker，每次执行一次
    /// </summary>
    public class OnceWorker : AbstractWorker, IOnceWorker
    {
        public OnceWorker(WorkerOption option, WorkerConfig config) : base(option, config)
        {
        }
        protected Task Execute(ConcurrentDictionary<string, string> data)
        {
            IBackRun backRun = (IBackRun)BrunTool.CreateInstance(_option.BrunType);
            backRun.Data = data;
            return backRun.Run(WorkerServer.Instance.StoppingToken);
        }
        /// <summary>
        /// 调用线程不用等待结果
        /// </summary>
        public void RunDontWait()
        {
            runTask = Run();
        }
        public async Task Run()
        {
            await Observe(WorkerEvents.StartRun);
            try
            {
                await Execute(_context.Items);
            }
            catch (Exception ex)
            {
                _context.ExceptFromRun(ex);
                await Observe(WorkerEvents.Except);
            }
            finally
            {
                await Observe(WorkerEvents.EndRun);
            }
        }
        public ConcurrentDictionary<string, string> GetData()
        {
            return _context.Items;
        }
        public T GetData<T>(string key)
        {
            throw new NotImplementedException();
            //var r = GetData(key);
            //if (r == null)
            //    return default;
            //return (T)r;
        }
    }
}
