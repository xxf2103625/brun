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
        /// 开始运行
        /// </summary>
        Excuting,
        /// <summary>
        /// 运行结束
        /// </summary>
        Exceted,
        ///<summary>
        ///开始暂停
        ///</summary>
        Pausing,
        /// <summary>
        /// 结束暂停
        /// </summary>
        Paused,
        /// <summary>
        /// 开始恢复
        /// </summary>
        Resuming,
        /// <summary>
        /// 结束恢复
        /// </summary>
        Resumed,
        /// <summary>
        /// 开始销毁
        /// </summary>
        Destroying,
        /// <summary>
        /// 结束销毁
        /// </summary>
        destroyed,
    }
}
