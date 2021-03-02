using System;
using System.Collections.Generic;
using System.Text;

namespace Brun.Enums
{
    //TODO Worker状态
    public enum WorkerState
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        Default,
        /// <summary>
        /// 运行中
        /// </summary>
        Excuting,

        /// <summary>
        /// 已暂停
        /// </summary>
        Paused,
    }
}
