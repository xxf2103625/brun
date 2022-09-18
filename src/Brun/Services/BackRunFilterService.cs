using Brun.BaskRuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Services
{
    public class BackRunFilterService : IBackRunFilterService
    {
        public List<Type> GetBackRunTypes()
        {
            return GetBackRunsFromBaseType(typeof(BackRun));
        }
        public List<Type> GetOnceBackRunTypes()
        {
            return GetBackRunsFromBaseType(typeof(OnceBackRun));
        }
        public List<Type> GetTimeBackRunTypes()
        {
            return GetBackRunsFromBaseType(typeof(TimeBackRun));
        }
        public List<Type> GetQueueBackRunTypes()
        {
            return GetBackRunsFromBaseType(typeof(QueueBackRun));
        }
        public List<Type> GetPlanBackRunTypes()
        {
            return GetBackRunsFromBaseType(typeof(PlanBackRun));
        }
        /// <summary>
        /// 查找用户自定义的BackRun
        /// </summary>
        /// <param name="baseType">继承的基类</param>
        /// <returns></returns>
        private List<Type> GetBackRunsFromBaseType(Type baseType)
        {
            var list = new List<Type>();
            var ass = Brun.Commons.BrunTool.GetReferanceAssemblies();
            foreach (var item in ass)
            {
                foreach (var t in item.GetTypes())
                {
                    if (t.IsSubclassOf(baseType) && !t.IsAbstract)
                    {
                        list.Add(t);
                    }
                }
            }
            return list;
        }
    }
}
