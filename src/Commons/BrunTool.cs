using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Brun.Commons
{
    public class BrunTool
    {
        public static object CreateInstance(Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args: args);
        }
        public static TType CreateInstance<TType>(params object[] args)
        {
            return (TType)Activator.CreateInstance(typeof(TType), args);
        }
    }
}
