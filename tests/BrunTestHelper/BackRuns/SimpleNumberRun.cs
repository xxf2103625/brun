﻿using Brun;
using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    public class SharedLock
    {
        public static object Nb_LOCK = new object();
    }
    /// <summary>
    /// 基础测试BackRun
    /// </summary>
    public class SimpleNumberRun : OnceBackRun
    {
        public SimpleNumberRun(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            for (int i = 0; i < 100; i++)
            {
                lock (SharedLock.Nb_LOCK)
                {
                    if (Data.TryGetValue("nb", out string v))
                    {
                        Data["nb"] = (int.Parse(v) + 1).ToString();
                    }
                    else
                    {
                        Data["nb"] = "1";
                    }
                    Console.WriteLine("SimpleNumberRun 计算出的结果：{0}", Data["nb"]);
                }

            }
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 测试逻辑前等待
    /// </summary>
    public class SimpNbDelayBefore : OnceBackRun
    {
        private static object SimpNbDelayBefore_LOCK = new object();

        public SimpNbDelayBefore(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("计算前的等待...");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            //await Task.Delay(TimeSpan.FromSeconds(3));
            for (int i = 0; i < 100; i++)
            {
                lock (SharedLock.Nb_LOCK)
                {
                    if (Data.TryGetValue("nb", out string v))
                    {
                        Data["nb"] = (int.Parse(v) + 1).ToString();
                    }
                    else
                    {
                        Data["nb"] = "0";
                    }
                    Console.WriteLine("SimpNbDelayBefore 计算出的结果：{0}", Data["nb"]);
                }
            }
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 测试逻辑后等待
    /// </summary>
    public class SimpNbDelayAfter : OnceBackRun
    {
        public SimpNbDelayAfter(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            for (int i = 0; i < 100; i++)
            {
                lock (SharedLock.Nb_LOCK)
                {
                    if (Data.TryGetValue("nb", out string v))
                    {
                        Data["nb"] = (int.Parse(v) + 1).ToString();
                    }
                    else
                    {
                        Data["nb"] = "0";
                    }
                    Console.WriteLine("SimpNbDelayAfter 计算出的结果：{0}", Data["nb"]);
                    
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Console.WriteLine("SimpNbDelayAfter 计算后已等待5秒...");
            return Task.CompletedTask;
        }

    }
    /// <summary>
    /// 测试逻辑前等待Task
    /// </summary>
    public class SimpNbDelayBeforeTask : OnceBackRun
    {
        public SimpNbDelayBeforeTask(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("SimpNbDelayBeforeTask 计算前的等待...");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            
            for (int i = 0; i < 100; i++)
            {
                lock (SharedLock.Nb_LOCK)
                {
                    if (Data.TryGetValue("nb", out string v))
                    {
                        Data["nb"] = (int.Parse(v) + 1).ToString();
                    }
                    else
                    {
                        Data["nb"] = "0";
                    }
                }
                
            }
            Console.WriteLine("SimpNbDelayBeforeTask 计算出的结果：{0}", Data["nb"]);
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 测试逻辑后等待 Task
    /// </summary>
    public class SimpNbDelayAfterTask : OnceBackRun
    {
        public SimpNbDelayAfterTask(OnceBackRunOption option) : base(option)
        {
        }

        public override Task Run(CancellationToken stoppingToken)
        {
            for (int i = 0; i < 100; i++)
            {
                lock (SharedLock.Nb_LOCK)
                {
                    if (Data.TryGetValue("nb", out string v))
                    {
                        Data["nb"] = (int.Parse(v) + 1).ToString();
                    }
                    else
                    {
                        Data["nb"] = "0";
                    }
                    Console.WriteLine(" SimpNbDelayAfterTask 计算出的结果：{0}", Data["nb"]);
                }
                
            }
            //await Task.Delay(TimeSpan.FromSeconds(3));
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Console.WriteLine(" SimpNbDelayAfterTask 等待3秒之后 {0}", Data["nb"]);
            return Task.CompletedTask;
        }
    }
}
