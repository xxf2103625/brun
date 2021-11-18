//using Brun.Commons;
//using Brun.Models;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using System.Text.Json;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Brun.Services
//{
//    //TODO 迁移到扩展类库
//    //TODO 执行时间可能超过1秒
//    public class SystemBackRun : BackRun
//    {
//        public readonly static string Worker_KEY = "brun_sys_worker";
//        public readonly static string SystemInfo_KEY = "system";
//        public readonly static string SystemRun_KEY = "system_run";
//        public override Task Run(CancellationToken stoppingToken)
//        {
//            DateTime now = DateTime.Now;
//            double cpuRage = MachineUtil.GetCPURate();
//            RamInfo ramInfo = MachineUtil.GetRamInfo();
//            if (!Data.ContainsKey(SystemInfo_KEY))
//            {
//                SystemInfo systemInfo = MachineUtil.GetMachineBaseInfo(Math.Round(ramInfo.Total / 1024f, 2));
//                Data.TryAdd(SystemInfo_KEY, JsonSerializer.Serialize(systemInfo));
//            }
//            SystemRunInfo runInfo = new SystemRunInfo();
//            runInfo.Time = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
//            runInfo.CpuRage = cpuRage;
//            runInfo.RamUse = Math.Round(ramInfo.Used, 2);
//            runInfo.ProcessRamUse = Math.Round(Process.GetCurrentProcess().WorkingSet64 / 1024f / 1024f, 2);
//            Console.WriteLine("Brun系统监控：" + runInfo);
//            if (Data.ContainsKey(SystemRun_KEY))
//            {
//                List<SystemRunInfo> runInfos = JsonSerializer.Deserialize<List<SystemRunInfo>>(Data[SystemRun_KEY]);
//                if (runInfos.Count >= 10)
//                {
//                    runInfos.RemoveAt(0);
//                }
//                runInfos.Add(runInfo);
//                Data[SystemRun_KEY] = JsonSerializer.Serialize(runInfos);
//            }
//            else
//            {
//                List<SystemRunInfo> runInfos = new List<SystemRunInfo>() { runInfo };
//                Data.TryAdd(SystemRun_KEY, JsonSerializer.Serialize(runInfos));
//            }
//            //await Task.CompletedTask;
//            return Task.CompletedTask;
//        }
//    }
//}
