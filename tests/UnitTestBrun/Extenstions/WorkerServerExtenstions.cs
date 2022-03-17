using Brun.Services;
using Brun.Workers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// Test专用
    /// </summary>
    public static class WorkerServerExtenstionsForMemoryTest
    {
        /// <summary>
        /// 创建OnceWorker
        /// </summary>
        /// <param name="workerServer"></param>
        /// <param name="config"></param>
        /// <param name="startNow"></param>
        /// <returns></returns>
        public static OnceWorker CreateOnceWorker(this WorkerServer workerServer, WorkerConfig config, bool startNow = true)
        {
            return CreateWorker<OnceWorker>(workerServer, config);
        }
        public static QueueWorker CreateQueueWorker(this WorkerServer workerServer, WorkerConfig config, bool startNow = true)
        {
            return CreateWorker<QueueWorker>(workerServer, config);
        }
        public static TimeWorker CreateTimeWorker(this WorkerServer workerServer, WorkerConfig config, bool startNow = true)
        {
            return CreateWorker<TimeWorker>(workerServer, config);
        }

        public static PlanWorker CreatePlanTimeWorker(this WorkerServer workerServer, WorkerConfig config, bool startNow = true)
        {
            return CreateWorker<PlanWorker>(workerServer, config);
        }
        public static TWorker CreateWorker<TWorker>(this WorkerServer workerServer, WorkerConfig config) where TWorker : AbstractWorker
        {
            return (TWorker)CreateWorker(workerServer, typeof(TWorker), config);
        }
        public static IWorker CreateWorker(this WorkerServer workerServer, Type workerType, WorkerConfig config)
        {
            if (config.Key == null)
                config.Key = Guid.NewGuid().ToString();
            if (config.Name == null)
                config.Name = workerType.Name;
            var workerService = workerServer.ServiceProvider.GetRequiredService<IWorkerService>();
            var worker = workerService.AddWorker(config, workerType);
            worker.Start();
            return worker;
        }
    }
}
