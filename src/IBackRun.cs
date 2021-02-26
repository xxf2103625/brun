using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 所有后台任务的接口
    /// </summary>
    public interface IBackRun
    {
        Task Run(CancellationToken stoppingToken);
    }
}
