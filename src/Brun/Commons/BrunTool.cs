using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static List<Assembly> GetReferanceAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
        public static (int, Assembly, string) LoadFile(string fileName)
        {
            //重复判断 //TODO 文件名可能和程序集名不一致
            if (fileName.EndsWith(".dll"))
            {
                string assName = fileName.Substring(0, fileName.Length - 4);
                if (AppDomain.CurrentDomain.GetAssemblies().Any(m => m.GetName().Name == assName))
                {
                    return (-1, null, "该程序集已加载");
                }
            }

            try
            {
                //卸载得创建新的AppDomain
                var ass = Assembly.LoadFile(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
                return (1, ass, "加载成功");
            }
            catch (Exception ex)
            {
                return (0, null, ex.Message);
            }
        }
    }
}
