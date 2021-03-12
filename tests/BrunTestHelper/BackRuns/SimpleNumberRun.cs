using Brun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunTestHelper.BackRuns
{
    /// <summary>
    /// 基础测试BackRun
    /// </summary>
    public class SimpleNumberRun : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            for (int i = 0; i < 100; i++)
            {
                if (Data.TryGetValue("nb", out string v))
                {
                    Data["nb"] = (int.Parse(v) + 1).ToString();
                }
                else
                {
                    Data["nb"] = "1";
                }
            }
            Console.WriteLine("计算出的结果：{0}", Data["nb"]);
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 测试逻辑前等待
    /// </summary>
    public class SimpNbDelayBefore : BackRun
    {
        public override async Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("计算前的等待...");
            await Task.Delay(TimeSpan.FromSeconds(3));
            for (int i = 0; i < 100; i++)
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
            Console.WriteLine("计算出的结果：{0}", Data["nb"]);
        }
    }
    /// <summary>
    /// 测试逻辑后等待
    /// </summary>
    public class SimpNbDelayAfter : BackRun
    {
        public override async Task Run(CancellationToken stoppingToken)
        {
            for (int i = 0; i < 100; i++)
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
            Console.WriteLine("计算出的结果：{0}", Data["nb"]);
            await Task.Delay(TimeSpan.FromSeconds(3));
            Console.WriteLine("计算后已等待5秒...");
        }
    }
    /// <summary>
    /// 测试逻辑前等待Task
    /// </summary>
    public class SimpNbDelayBeforeTask : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            Console.WriteLine("计算前的等待...");
            Thread.Sleep(TimeSpan.FromSeconds(3));
            for (int i = 0; i < 100; i++)
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
            Console.WriteLine("计算出的结果：{0}", Data["nb"]);
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 测试逻辑后等待 Task
    /// </summary>
    public class SimpNbDelayAfterTask : BackRun
    {
        public override Task Run(CancellationToken stoppingToken)
        {
            for (int i = 0; i < 100; i++)
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
            Console.WriteLine("计算出的结果：{0}", Data["nb"]);
            //await Task.Delay(TimeSpan.FromSeconds(3));
            Thread.Sleep(TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }
    }
}
