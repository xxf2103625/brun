using Brun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{

    public class DataBackRun : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            //await Task.Delay(TimeSpan.FromSeconds(0.2), stoppingToken);
            lock (SharedLock.Nb_LOCK)
            {
                if (Data.TryGetValue("a", out string s))
                {
                    var ts = s;
                }
                else
                {
                    Data["a"] = "1";
                }
            }
            
            return Task.CompletedTask;
        }
    }
    public class ErrorBackRun4 : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            //await Task.Delay(TimeSpan.FromSeconds(0.2),stoppingToken);
            //Thread.Sleep(TimeSpan.FromSeconds(1));
            lock (SharedLock.Nb_LOCK)
            {
                if (Data.TryGetValue("a", out string s))
                {
                    var ts = s;
                }
                else
                {
                    Data["a"] = "1";
                }
            }
            

            throw new NotImplementedException("测试异常");
        }
    }
    public class ErrorBackRun : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            //await Task.Delay(TimeSpan.FromSeconds(0.2),stoppingToken);
            //Thread.Sleep(TimeSpan.FromSeconds(1));
            lock (SharedLock.Nb_LOCK)
            {
                if (Data.TryGetValue("a", out string s))
                {
                    var ts = s;
                }
                else
                {
                    Data["a"] = "1";
                }
            }
            

            throw new NotImplementedException("测试异常");
        }
    }
    public class ErrorLongBackRun : BackRun
    {
        private static object ErrorLongBackRun_LOCK = new object();
        public override Task Run(CancellationToken stoppingToken)
        {
            //await Task.Delay(TimeSpan.FromSeconds(0.2),stoppingToken);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            lock (SharedLock.Nb_LOCK)
            {
                if (Data.TryGetValue("a", out string s))
                {
                    var ts = s;
                }
                else
                {
                    Data["a"] = "1";
                }
            }
            throw new NotImplementedException("测试异常");
        }
    }
    public class ErrorBackRun3 : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            lock (SharedLock.Nb_LOCK)
            {
                if (Data.TryGetValue("a", out string s))
                {
                    var ts = s;
                }
                else
                {
                    Data["a"] = "1";
                }
            }
            

            throw new NotImplementedException("测试异常");
        }
    }
}
