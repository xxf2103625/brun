﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Contexts
{
    /// <summary>
    /// 每个BackRun运行时的上下文
    /// </summary>
    public class BrunContext : IDisposable
    {
        Type brunType;
        public BrunContext(Type backRunType)
        {
            brunType = backRunType;
        }
        public Type BrunType => brunType;
        public long StartNb { get; set; }
        public int ExceptNb { get; set; }
        public long EndNb { get; set; }
        public string Message { get; set; }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
