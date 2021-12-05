using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Models
{
    /// <summary>
    /// Antd Pro表格返回数据
    /// </summary>
    public class TableResult
    {
        public TableResult() { }
        public TableResult(object data, int total, bool success)
        {
            Data = data;
            Total = total;
            Success = success;
        }
        public object Data { get; set; }
        public int Total { get; set; }
        public bool Success { get; set; }
    }
}
