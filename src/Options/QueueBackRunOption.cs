using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public class QueueBackRunOption
    {
        private ConcurrentQueue<string> _queue;
        public QueueBackRunOption(string id = null)
        {
            _queue = new ConcurrentQueue<string>();
            if (id == null)
            {
                Id = Guid.NewGuid().ToString();
            }
        }
        public string Id { get; set; }
        public ConcurrentQueue<string> Queue => _queue;
    }
}
