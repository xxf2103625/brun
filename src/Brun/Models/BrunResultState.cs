using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Models
{
    /// <summary>
    /// 定义常用结果状态码
    /// </summary>
    public enum BrunResultState
    {
        /// <summary>
        /// 未知异常
        /// </summary>
        UnKnow = 0,
        /// <summary>
        /// 操作成功
        /// </summary>
        Success = 1,
        /// <summary>
        /// 操作失败
        /// </summary>
        Error = 2,
        /// <summary>
        /// Worker未运行
        /// </summary>
        NotRunning = 6,
        /// <summary>
        /// 找不到指定数据/对象
        /// </summary>
        NotFound = 7,
        /// <summary>
        /// Id被占用
        /// </summary>
        IdBeUsed = 8,
    }
}
