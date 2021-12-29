using Brun.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// WorkerServer配置
    /// </summary>
    public class WorkerServerOption
    {
        /// <summary>
        /// WorkerServer Start之前的配置
        /// </summary>
        public Action<WorkerServer> ConfigreWorkerServer { get; set; }
        /// <summary>
        /// 程序启动时配置初始化Worker和BackRun
        /// </summary>
        public Action<IWorkerService> InitWorkers { get; set; }

        /// <summary>
        /// 储存类型
        /// </summary>
        public WorkerStoreType StoreType { get; set; }
        /// <summary>
        /// 持久化连接字符串
        /// </summary>
        public string StoreConn { get; set; }
        /// <summary>
        /// 扩展库用的服务注册/替换
        /// </summary>
        public Action<IServiceCollection> ServicesConfigure { get; set; }
        /// <summary>
        /// UI登录用户名，默认admin
        /// </summary>
        public string UserName { get; set; } = "admin";
        /// <summary>
        /// UI登录密码，默认admin
        /// </summary>
        public string Password { get; set; } = "admin";
        /// <summary>
        /// true：Linux下创建Pid文件方便监控进程（Monit需要pid文件）
        /// </summary>
        public bool CreateLinuxPidFile { get; set; } = true;
    }
    /// <summary>
    /// 储存类型
    /// </summary>
    public enum WorkerStoreType
    {
        None,
        Memory,
        /// <summary>
        /// SqlSugar支持的数据库
        /// </summary>
        Store,
        Redis,
        MongoDb
    }
}
