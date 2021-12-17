using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Options
{
    public class QueueBackRunOption : BackRunOption
    {
        public QueueBackRunOption() : this(null, null)
        {

        }
        public QueueBackRunOption(string id, string name) : base(id, name)
        {
        }
    }
}
