using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTestBrun
{
    [TestClass]
    public class CommonTest
    {
        [TestMethod]
        public void TestJsonSer()
        {
            System.Text.Json.JsonSerializerOptions jsonOpt = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);
            System.Text.Json.JsonSerializerOptions jsonOpt2 = new System.Text.Json.JsonSerializerOptions();
            jsonOpt2.PropertyNameCaseInsensitive = true;
            jsonOpt2.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            DomainApiResult model = new DomainApiResult();
            model.CodeTest = 200;
            model.Msg = "测试";
            model.Data = 0;
            var modelStr = JsonSerializer.Serialize(model, jsonOpt2);
            //string str = "{\"Code\":200,\"Msg\":\"\u6D4B\u8BD5\",\"Data\":0}";
            string str2 = "{\"code\":200,\"msg\":\"\u6D4B\u8BD5\",\"data\":0}";
            var m = JsonSerializer.Deserialize<DomainApiResult>(str2, jsonOpt2);
        }
        [TestMethod]
        public void RandomString()
        {
            string allStr = "abcdefghijklmnopqrstuvwxyz23456789";
            char[] arr = allStr.ToCharArray();
            Random random = new Random();
            string r = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                r +=arr[random.Next(arr.Length)];
            }
            Console.WriteLine(r);
        }
    }
    /// <summary>
    /// 域名状态结果
    /// </summary>
    public class DomainApiResult
    {
        /// <summary>
        /// 200 api正常
        /// 错误码	说明
        ///500
        ///	参数错误
        ///511
        ///	appkey不合法
        ///513
        ///	套餐余量不足
        ///514	请求频繁
        ///515	接口不存在
        ///520 url为空
        /// </summary>
        public int CodeTest { get; set; }
        public string Msg { get; set; }
        /// <summary>
        /// 0:域名正常，1：非官方网址，请确认是否继续访问，2：域名已封杀，3：提示如需浏览，请长按网址复制后使用浏览器打开
        /// </summary>
        public int Data { get; set; }
    }
}
/*
{"Code":200,"Msg":"\u6D4B\u8BD5","Data":0}
*/
