using Brun.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brun.Services
{
    public class BrunMonitor
    {
        private IWorkerServer _workerServer;
        public BrunMonitor(IWorkerServer workerServer)
        {
            _workerServer = workerServer;
        }
        public IWorker GetWorkerByKey(string key)
        {
            return _workerServer.GetWorker(key);
        }
        public BrunInfo GetBrunInfo()
        {
            BrunInfo brunInfo = new BrunInfo();
            brunInfo.StartTime = _workerServer.StartTime;

            var infos = _workerServer.Worders.Select(m => new WorkerInfo
            {
                TypeName = m.GetType().Name,
                Key = m.Context.Key,
                Name = m.Context.Name,
                Tag = m.Context.Tag,
                BrunTypes = m.BrunTypes.Select(w => w.Name),
                RunningNb = m.RunningTasks.Count,
                StartNb = m.Context.startNb,
                ExceptNb = m.Context.exceptNb,
                EndNb = m.Context.endNb,
            });
            brunInfo.Workers = infos.OrderBy(m => m.TypeName).ToList();
            return brunInfo;
        }
    }
}
