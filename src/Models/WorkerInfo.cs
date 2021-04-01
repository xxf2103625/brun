using System;
using System.Collections.Generic;
using System.Text;

namespace Brun.Models
{
    public class WorkerInfo
    {
        public string TypeName { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public IEnumerable<string> BrunTypes { get; set; }
        public long StartNb { get; set; }
        public int ExceptNb { get; set; }
        public long EndNb { get; set; }
        public int RunningNb { get; set; }
    }
}
