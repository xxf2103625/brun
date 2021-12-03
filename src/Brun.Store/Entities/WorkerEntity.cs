using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Store.Entities
{
    /// <summary>
    /// Worker持久化数据实体
    /// </summary>
    public class WorkerEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        //[ColumnDescription()]
        public string Id { get; set; }
        /// <summary>
        /// Worker类型，字符串OnceWorker/TimeWorker...
        /// </summary>
        public string Type { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDataType ="time with time zone")]
        public DateTimeOffset Ct { get; set; } = DateTimeOffset.Now;
    }
}
