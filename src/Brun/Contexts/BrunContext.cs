using Brun.BaskRuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Contexts
{
    /// <summary>
    /// 每个BackRun运行时的上下文
    /// </summary>
    public class BrunContext : IDisposable
    {
        private IBackRun _backRun;
        public BrunContext(IBackRun backRun)
        {
            this._backRun = backRun;
        }
        public IBackRun BackRun => _backRun;
        public long StartNb { get; set; }
        public int ExceptNb { get; set; }
        public long EndNb { get; set; }
        public string Message { get; set; }
        //TODO 每次Run结束释放资源
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
