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
        //public TableResult() { }
        public TableResult(object data, int total, bool success = true)
        {
            Data = data;
            Total = total;
            Success = success;
        }
        public object Data { get; set; }
        public int Total { get; set; }
        public bool Success { get; set; }
    }

    public class InfoResult
    {
        public InfoResult(){}

        public InfoResult(bool success, object data, string msg = null)
        {
            this.Success = success;
            this.Data = data;
            this.Msg = msg;
        }
        public object Data { get; set; }
        public bool Success { get; set; }
        public string Msg { get; set; }

        public static InfoResult Ok(object data)
        {
            return new InfoResult(true, data);
        }

        public static InfoResult Error(object err,string msg=null)
        {
            return new InfoResult(false, err,msg);
        }
    }
}
