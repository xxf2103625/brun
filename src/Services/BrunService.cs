//using Brun.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Brun.Services
//{
//    /// <summary>
//    /// TODO 封装Brun常用操作，避免直接使用WorkerServer,为拆分Client做准备
//    /// </summary>
//    public class BrunService
//    {
//        private IWorkerServer _workerServer;
//        public BrunService(IWorkerServer workerServer)
//        {
//            _workerServer = workerServer;
//        }
//        /// <summary>
//        /// Brun概况
//        /// </summary>
//        /// <returns></returns>
//        public BrunInfo GetBrunInfo()
//        {
//            BrunInfo brunInfo = new BrunInfo();
//            brunInfo.StartTime = _workerServer.StartTime;
//            IEnumerable<WorkerInfo> infos = _workerServer.Worders.Select(WorkerInfoCast);
//            brunInfo.Workers = infos.OrderBy(m => m.TypeName).ToList();
//            return brunInfo;
//        }
//        /// <summary>
//        /// 按key查询
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public WorkerInfo GetWorkerInfoByKey(string key)
//        {
//            IWorker worker = _workerServer.GetWorker(key);
//            return WorkerInfoCast(worker);
//        }
//        /// <summary>
//        /// 按name查询
//        /// </summary>
//        /// <param name="name"></param>
//        /// <returns></returns>
//        public IEnumerable<WorkerInfo> GetWorkerInfoByName(string name)
//        {
//            return _workerServer.GetWokerByName(name).Select(WorkerInfoCast);
//        }
//        /// <summary>
//        /// 启动
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public void StartWorker(string key)
//        {
//            _workerServer.GetWorker(key).Start();
//        }
//        /// <summary>
//        /// 停止
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        public void StopWorker(string key)
//        {
//            _workerServer.GetWorker(key).Stop();
//        }
//        /// <summary>
//        /// 注销
//        /// </summary>
//        /// <param name="key"></param>
//        public void DisposWorker(string key)
//        {
//            _workerServer.GetWorker(key).Dispose();
//        }
//        /// <summary>
//        /// 获取系统信息
//        /// </summary>
//        /// <returns></returns>
//        public SystemInfo GetSystemInfo()
//        {
//            IWorker worker = _workerServer.GetWorker(SystemBackRun.Worker_KEY);
//            if (worker == null)
//            {
//                return null;
//            }
//            if (((IPlanTimeWorker)worker).Context.Items.TryGetValue(SystemBackRun.SystemInfo_KEY, out string sysInfo))
//            {
//                return System.Text.Json.JsonSerializer.Deserialize<SystemInfo>(sysInfo);
//            }
//            else
//            {
//                return null;
//            }
//        }
//        /// <summary>
//        ///  获取系统监控信息
//        /// </summary>
//        /// <returns></returns>
//        public List<SystemRunInfo> GetSystemRunInfos()
//        {
//            IWorker worker = _workerServer.GetWorker(SystemBackRun.Worker_KEY);
//            if (worker == null)
//            {
//                return null;
//            }
//            if (((IPlanTimeWorker)worker).Context.Items.TryGetValue(SystemBackRun.SystemRun_KEY, out string runInfos))
//            {
//                return System.Text.Json.JsonSerializer.Deserialize<List<SystemRunInfo>>(runInfos);
//            }
//            else
//            {
//                return null;
//            }
//        }
//        /// <summary>
//        /// 隐藏IWorker，返回WorkerInfo
//        /// </summary>
//        private Func<IWorker, WorkerInfo> WorkerInfoCast = new Func<IWorker, WorkerInfo>(m =>
//           {
//               return new WorkerInfo()
//               {
//                   TypeName = m.GetType().Name,
//                   Key = m.Context.Key,
//                   Name = m.Context.Name,
//                   //BrunTypes = m.BrunTypes.Select(w => w.Name),
//                   //RunningNb = m.RunningTasks.Count,
//                   State = m.Context.State,
//                   StartNb = m.Context.startNb,
//                   ExceptNb = m.Context.exceptNb,
//                   EndNb = m.Context.endNb,
//               };
//           });
//    }
//}
