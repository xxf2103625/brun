//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Brun.Options
//{
//    /// <summary>
//    /// 用于构造Worker
//    /// </summary>
//    [Obsolete("移除",false)]
//    public class WorkerOption
//    {
//        /// <summary>
//        /// 默认的BrunType
//        /// </summary>
//        public virtual Type DefaultBrunType => BrunTypes[0];
//        public IList<Type> BrunTypes { get; set; }
//        public Type WorkerType { get; set; }
//        public string Key { get; set; }
//        public string Name { get; set; }
//        public string Tag { get; set; }
//        /// <summary>
//        /// run内部还是需要自己加锁保证线程安全
//        /// </summary>
//        public ConcurrentDictionary<string, string> Data { get; set; }
//    }
//}
