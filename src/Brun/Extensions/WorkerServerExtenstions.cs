using Brun.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public static class WorkerServerExtenstions
    {
        /// <summary>
        /// 创建OnceWorker
        /// </summary>
        /// <param name="workerServer"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static OnceWorker CreateOnceWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new OnceWorker(config);
            WorkerServer.Instance.Worders.Add(worker);
            return worker;
        }
        public static SynchroWorker CreateSynchroWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new SynchroWorker(config);
            WorkerServer.Instance.Worders.Add(worker);
            return worker;
        }
        public static QueueWorker CreateQueueWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new QueueWorker(config);
            WorkerServer.Instance.Worders.Add(worker);
            return worker;
        }
        //public static OnceWorker CreateOnceWorker(this WorkerServer workerServer,string key,string name,string tag)
        //{

        //}
        public static TimeWorker CreateTimeWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new TimeWorker(config);
            WorkerServer.Instance.Worders.Add(worker);
            return worker;
        }
        
        public static PlanWorker CreatePlanTimeWorker(this WorkerServer workerServer,WorkerConfig config)
        {
            var worker = new PlanWorker(config);
            WorkerServer.Instance.Worders.Add(worker);
            return worker;
        }
    }
}
