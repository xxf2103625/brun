using System;
using System.Collections.Generic;
using System.Text;

namespace Brun.Enums
{
    /// <summary>
    /// Worker状态
    /// </summary>
    public enum WorkerState
    {
        /// <summary>
        /// 未激活
        /// </summary>
        Default,
        /// <summary>
        /// 运行中
        /// </summary>
        Started,
        /// <summary>
        /// 已停止
        /// </summary>
        Stoped,
    }
}
