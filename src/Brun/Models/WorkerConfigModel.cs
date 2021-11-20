using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Models
{
    public class WorkerConfigModel
    {
        /// <summary>
        /// key为空时为随机字符串
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 为空时为类型名称字符串
        /// </summary>
        public string Name { get; set; }
    }
}
