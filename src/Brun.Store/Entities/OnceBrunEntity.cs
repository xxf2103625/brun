using Brun.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Store.Entities
{
    public class OnceBrunEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string? Id { get; set; }
        public string? Name { get; set; }
        /// <summary>
        /// 所属WorkerId
        /// </summary>
        public string? WorkerId { get; set; }
        /// <summary>
        /// 类型全名
        /// </summary>
        public string? FullTypeName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string? TypeName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public BackRunState State { get; set; }
    }
}
