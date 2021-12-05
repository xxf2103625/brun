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
        private ConcurrentQueue<string> _queue;
        public QueueBackRunOption() : base(null, null)
        {

        }
        public QueueBackRunOption(string id, string name) : base(id, name)
        {
            _queue = new ConcurrentQueue<string>();
        }

        //public QueueBackRunOption(string id = null)
        //{
        //    _queue = new ConcurrentQueue<string>();
        //    if (id == null)
        //    {
        //        Id = Guid.NewGuid().ToString();
        //    }
        //}
        public ConcurrentQueue<string> Queue => _queue;
    }
}
