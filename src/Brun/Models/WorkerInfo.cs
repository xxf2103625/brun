﻿using Brun.Enums;
using Brun.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brun.Models
{
    public class WorkerInfo
    {
        /// <summary>
        /// Worker类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public WorkerType WorkerType => string.IsNullOrEmpty(TypeName) ? throw new BrunException(BrunErrorCode.TypeError, "TypeName is null or empty.") : Commons.BrunTool.GetWorkerType(TypeName);
        /// <summary>
        /// Worker唯一标识Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Worker名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 包含的BackRun类型
        /// </summary>
        public IEnumerable<string> BrunTypes { get; set; }
        /// <summary>
        /// 开始Task的数量
        /// </summary>
        public long StartNb { get; set; }
        /// <summary>
        /// 异常Task数量
        /// </summary>
        public int ExceptNb { get; set; }
        /// <summary>
        /// 完成Task数量
        /// </summary>
        public long EndNb { get; set; }
        /// <summary>
        /// 正在运行的Task数量
        /// </summary>
        public int RunningNb { get; set; }
        /// <summary>
        /// Worker状态
        /// </summary>
        public WorkerState State { get; set; }
    }
}
