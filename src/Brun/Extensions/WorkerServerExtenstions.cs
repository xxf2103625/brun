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
            workerServer.Worders.Add(worker.Key,worker);
            return worker;
        }
        public static SynchroWorker CreateSynchroWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new SynchroWorker(config);
            workerServer.Worders.Add(worker.Key,worker);
            return worker;
        }
        public static QueueWorker CreateQueueWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new QueueWorker(config);
            workerServer.Worders.Add(worker.Key,worker);
            return worker;
        }
        //public static OnceWorker CreateOnceWorker(this WorkerServer workerServer,string key,string name,string tag)
        //{

        //}
        public static TimeWorker CreateTimeWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new TimeWorker(config);
            workerServer.Worders.Add(worker.Key,worker);
            return worker;
        }

        public static PlanWorker CreatePlanTimeWorker(this WorkerServer workerServer, WorkerConfig config)
        {
            var worker = new PlanWorker(config);
            workerServer.Worders.Add(worker.Key,worker);
            return worker;
        }
        public static TWorker CreateWorker<TWorker>(this WorkerServer workerServer, WorkerConfig config) where TWorker : AbstractWorker
        {
            var worker = Commons.BrunTool.CreateInstance<TWorker>(config);
            return worker;
        }
        public static IWorker CreateWorker(this WorkerServer workerServer, Type workerType, WorkerConfig config)
        {
            var worker = (IWorker)Commons.BrunTool.CreateInstance(workerType, config);
            return worker;
        }
    }
}
