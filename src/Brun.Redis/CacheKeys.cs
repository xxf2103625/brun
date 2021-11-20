using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Redis
{
    public class CacheKeys
    {
        /// <summary>
        /// List的key,储存OnceWorker集合
        /// </summary>
        public static string OnceWorkKey { get; set; } = "onceworker_list";
        public static string WorkerKeyPre { get; set; } = "wid_";
    }
}
