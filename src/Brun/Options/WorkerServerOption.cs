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
        public Action<WorkerServer> WorkerServer { get; set; }
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
        /// UI登录用户名，默认brun
        /// </summary>
        public string UserName { get; set; } = "admin";
        /// <summary>
        /// UI登录密码，默认brun
        /// </summary>
        public string Password { get; set; } = "admin";
    }
    /// <summary>
    /// 储存类型
    /// </summary>
    public enum WorkerStoreType
    {
        None,
        Memory,
        Store,
        Redis
    }
}
