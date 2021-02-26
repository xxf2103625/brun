using System;
using System.Collections.Generic;
using System.Text;

namespace Brun
{
    /// <summary>
    /// 用于构造Worker
    /// </summary>
    public class WorkerOption
    {
        public Type BrunType { get; set; }
        public Type WorkerType { get; set; }
        public string WorkerTypeName { get; set; }
        public string Key { get; set; }
        public string Name { get; internal set; }
        public string Tag { get; internal set; }
        public IDictionary<string,object> Data { get; set; }
    }
}
