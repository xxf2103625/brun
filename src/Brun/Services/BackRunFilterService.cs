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
        //private static List<Type> systemBackRun = new List<Type>() { typeof(BackRun), typeof(TimeBackRun), typeof(QueueBackRun), typeof(PlanBackRun) };
        public List<Type> GetBackRunTypes()
        {
            var list = new List<Type>();
            var ass = Brun.Commons.BrunTool.GetReferanceAssemblies();
            foreach (var item in ass)
            {
                foreach (var t in item.GetTypes())
                {
                    if (t.IsSubclassOf(typeof(BackRun)) && !t.IsAbstract)
                    {
                        list.Add(t);
                    }
                }
            }
            return list;
        }
        public List<Type> GetOnceBackRunTypes()
        {
            return GetBackRunTypes().Where(m => !m.IsSubclassOf(typeof(TimeBackRun)) && !m.IsSubclassOf(typeof(QueueBackRun)) && !m.IsSubclassOf(typeof(PlanBackRun))).ToList();
        }
        public List<Type> GetTimeBackRunTypes()
        {
            return GetBackRunTypes().Where(m => m.IsSubclassOf(typeof(TimeBackRun))).ToList();
        }
        public List<Type> GetQueueBackRunTypes()
        {
            return GetBackRunTypes().Where(m => m.IsSubclassOf(typeof(QueueBackRun))).ToList();
        }
        public List<Type> GetPlanBackRunTypes()
        {
            return GetBackRunTypes().Where(m => m.IsSubclassOf(typeof(PlanBackRun))).ToList();
        }
    }
}
