using Brun.Commons;
using Brun.Contexts;
using Brun.Enums;
using Brun.Observers;
using Brun.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Workers
{
    /// <summary>
    /// 同步Worker，同一个Worker内，backrun串行运行
    /// </summary>
    public class SynchroWorker : OnceWorker
    {
        public SynchroWorker(WorkerConfig config) : base(config)
        {
        }
        private SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        protected override void Init()
        {
            if (string.IsNullOrEmpty(this._config.Key))
            {
                this._config.Key = Guid.NewGuid().ToString();
            }
            if (string.IsNullOrEmpty(this._config.Name))
            {
                this._config.Name = nameof(SynchroWorker);
            }
            _logger.LogInformation($"SynchroWorker with key '{this.Key}' is init.");
        }
        public override async Task StartBrun(Type brunType)
        {
            semaphoreSlim.Wait();
            await base.StartBrun(brunType);
            semaphoreSlim.Release();
        }
    }

}
