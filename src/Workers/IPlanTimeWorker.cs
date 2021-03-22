using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Workers
{
    public interface IPlanTimeWorker : IWorker
    {
        /// <summary>
        /// 启动，已经单独开了线程
        /// </summary>
        void Start();
    }
}
