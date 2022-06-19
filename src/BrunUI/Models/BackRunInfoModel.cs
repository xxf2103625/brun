using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Models
{
    public class BackRunInfoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string TypeFullName { get; set; }
        public string WorkerKey { get; set; }
        public string WorkerName { get; set; }
        public long StartTimes { get; internal set; }
        public long ErrorTimes { get; internal set; }
        public long EndTimes { get; internal set; }
    }
    public class TimeBackRunInfoModel: BackRunInfoModel
    {
        public double TotalSeconds { get; set; }
    }
}
