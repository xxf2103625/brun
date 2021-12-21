using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Exceptions
{
    /// <summary>
    /// Brun异常码
    /// </summary>
    public enum BrunErrorCode
    {
        /// <summary>
        /// 类型异常
        /// </summary>
        TypeError,
        /// <summary>
        /// id/key重复
        /// </summary>
        AllreadyKey,
        /// <summary>
        /// 不包含指定key
        /// </summary>
        NotFoundKey,
        /// <summary>
        /// 对象不能为null
        /// </summary>
        ObjectIsNull,
        /// <summary>
        /// 内存服务异常
        /// </summary>
        MemoryServiceError,
        /// <summary>
        /// Store服务异常
        /// </summary>
        StoreServiceError,
    }
}
