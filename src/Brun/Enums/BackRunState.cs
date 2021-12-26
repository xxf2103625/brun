using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Enums
{
    /// <summary>
    /// 是否添加到了Worker，主要在持久化中用，可能数据库中有但程序集找不到对象类型
    /// </summary>
    public enum BackRunState
    {
        /// <summary>
        /// 在线
        /// </summary>
        Online,
        /// <summary>
        /// 离线
        /// </summary>
        Offline
    }
}
