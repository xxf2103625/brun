using Brun.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Models
{
    public class WorkerModel
    {
        /// <summary>
        /// key为空时为随机字符串
        /// </summary>
        public string? Key { get; set; }
        /// <summary>
        /// 为空时为类型名称字符串
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Worker类型
        /// </summary>
        public WorkerType WorkerType { get; set; }
    }
}
