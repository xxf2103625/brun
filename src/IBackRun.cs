using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun
{
    /// <summary>
    /// 所有后台任务的接口,直接继承这个无法使用自定义Data
    /// </summary>
    public interface IBackRun
    {
        Task Run(CancellationToken stoppingToken);
    }
}
