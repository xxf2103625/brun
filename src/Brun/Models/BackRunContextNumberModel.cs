using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Models
{
    public class BackRunContextNumberModel
    {
        public string BackRunId { get; set; }
        /// <summary>
        /// 已开始运行的数量
        /// </summary>
        public long Start { get; set; }
        /// <summary>
        /// 异常数量
        /// </summary>
        public long Except { get; set; }
        /// <summary>
        /// 运行中的数量
        /// </summary>
        public long Running { get; set; }
    }
}
