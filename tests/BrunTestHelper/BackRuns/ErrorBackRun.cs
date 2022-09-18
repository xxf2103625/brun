using Brun;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{

    public class DataBackRun : OnceBackRun
    {
        public DataBackRun(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            //await Task.Delay(TimeSpan.FromSeconds(0.2), stoppingToken);
            //Thread.Sleep(TimeSpan.FromSeconds(0.2));
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
    public class ErrorBackRun4 : OnceBackRun
    {
        public ErrorBackRun4(OnceBackRunOption option) : base(option)
        {
        }

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
    public class ErrorBackRun : OnceBackRun
    {
        public ErrorBackRun(OnceBackRunOption option) : base(option)
        {
        }

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
    public class ErrorLongBackRun : OnceBackRun
    {
        public ErrorLongBackRun(OnceBackRunOption option) : base(option)
        {
        }

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
    public class ErrorBackRun3 : OnceBackRun
    {
        public ErrorBackRun3(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Thread.Sleep(TimeSpan.FromSeconds(0.1));
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
