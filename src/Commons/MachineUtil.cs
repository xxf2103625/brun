using Brun.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brun.Commons
{
    /// <summary>
    /// 获取服务器信息
    /// </summary>
    public static class MachineUtil
    {
        /// <summary>
        /// 获取资源使用信息
        /// </summary>
        /// <returns></returns>
        public static MachineRunTimeInfo GetMachineUseInfo()
        {
            //Process.GetCurrentProcess().TotalProcessorTime
            Console.WriteLine($"当前进程:{Process.GetCurrentProcess().ProcessName}");
            double ramUse = Process.GetProcesses().Sum(m => m.WorkingSet64 / 1024f / 1024f / 1024f);
            Console.WriteLine($"系统总内存：{(Environment.WorkingSet / 1024f / 1024f / 1024f).ToString("f2")}GB,使用内存：{ramUse.ToString("f2")}GB,当前进程占用内存：{(Process.GetCurrentProcess().WorkingSet64 / 1024f / 1024f / 1024f).ToString("f2")}GB");
            RamInfo ramInfo = GetRamInfo();
            return new MachineRunTimeInfo(
                1,
                //Math.Ceiling(ramInfo.Total / 1024f).ToString() + " GB", // 总内存
                Math.Ceiling(100 * ramInfo.Used / ramInfo.Total), // 内存使用率
                GetCPURate() // cpu使用率
            );
        }

        /// <summary>
        /// 获取基本参数
        /// </summary>
        /// <returns></returns>
        public static SystemInfo GetMachineBaseInfo(double totalRam)
        {
            //double totalRam = GetRamInfo().Total;
            return new SystemInfo(
                    Environment.MachineName, // HostName
                    RuntimeInformation.OSDescription, // 系统名称
                    Environment.OSVersion.Platform.ToString() + " " + RuntimeInformation.OSArchitecture.ToString(), // 系统架构
                    RuntimeInformation.FrameworkDescription, // .NET和Furion版本
                    Environment.ProcessorCount,//.ToString() + "核", // CPU核心数
                    totalRam
            );
        }
        /// <summary>
        /// 是否Linux
        /// </summary>
        /// <returns></returns>
        private static bool IsUnix()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        /// <summary>
        /// 获取CPU使用率
        /// </summary>
        /// <returns></returns>
        public static double GetCPURate()
        {
            string cpuRate;
            if (IsUnix())
            {
                var output = ShellUtil.Bash("top -b -n1 | grep \"Cpu(s)\" | awk '{print $2 + $4}'");
                cpuRate = output.Trim();
            }
            else
            {
                var output = ShellUtil.Cmd("wmic", "cpu get LoadPercentage");
                cpuRate = output.Replace("LoadPercentage", string.Empty).Trim();
            }

            if (double.TryParse(cpuRate, out double rate))
            {
                return rate;
            }
            else
            {
                return 0;
            }

        }
        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        public static RamInfo GetRamInfo()
        {

            if (IsUnix())
            {
                var output = ShellUtil.Bash("free -m");
                var lines = output.Split('\n');
                char[] chars = new char[] { ' ' };
                var memory = lines[1].Split(chars, options: StringSplitOptions.RemoveEmptyEntries);
                return new RamInfo
                {
                    Total = double.Parse(memory[1]),
                    Used = double.Parse(memory[2]),
                    Free = double.Parse(memory[3])
                };
            }
            else
            {
                var output = ShellUtil.Cmd("wmic", "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value");
                var lines = output.Trim().Split('\n');
                char[] chars = new char[] { '=' };
                var freeMemoryParts = lines[0].Split(chars, StringSplitOptions.RemoveEmptyEntries);
                var totalMemoryParts = lines[1].Split(chars, StringSplitOptions.RemoveEmptyEntries);
                var total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024f, 2);
                var free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024f, 2);
                return new RamInfo
                {
                    Total = total,
                    Free = free,
                    Used = total - free
                };
            }
        }
    }

    /// <summary>
    /// 系统Shell命令
    /// </summary>
    public class ShellUtil
    {
        /// <summary>
        /// Bash命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string Bash(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Dispose();
            return result;
        }

        /// <summary>
        /// cmd命令
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Cmd(string fileName, string args)
        {
            string output = string.Empty;
            var info = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = args,
                RedirectStandardOutput = true
            };
            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }
            return output;
        }
    }
    public class MachineInfo
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 操作系统名称
        /// </summary>
        public string SystemOs { get; set; }
        /// <summary>
        /// 操作系统架构
        /// </summary>
        public string OsArchitecture { get; set; }
        /// <summary>
        /// dotnet版本信息
        /// </summary>
        public string DotnetDescription { get; set; }
        /// <summary>
        /// CPU核心数
        /// </summary>
        public int ProcessorCount { get; set; }
        /// <summary>
        /// 总内存mb
        /// </summary>
        public double TotalRam { get; set; }

        public MachineInfo(string hostName, string systemOs, string osArchitecture, string dotnetDescription, int processorCount, double totalRam)
        {
            HostName = hostName;
            SystemOs = systemOs;
            OsArchitecture = osArchitecture;
            ProcessorCount = processorCount;
            DotnetDescription = dotnetDescription;
            ProcessorCount = processorCount;
            TotalRam = totalRam;
        }
        public override string ToString()
        {
            return $"HostName:{HostName},SystemOs:{SystemOs},OsArchitecture:{OsArchitecture},DotnetDescription:{DotnetDescription},ProcessorCount:{ProcessorCount},TotalRam:{TotalRam}";
        }
    }
    public class RamInfo
    {
        public double Total { get; set; }
        public double Used { get; set; }
        public double Free { get; set; }
    }
    /// <summary>
    /// 服务器实时消息
    /// </summary>
    public class MachineRunTimeInfo
    {
        /// <summary>
        /// 服务器实时消息
        /// </summary>
        /// <param name="cpuRate">Cpu使用率</param>
        /// <param name="ramUse">使用内存</param>
        /// <param name="processRamUse">进程使用内存</param>
        public MachineRunTimeInfo(double cpuRate, double ramUse, double processRamUse)
        {
            CpuRate = cpuRate;
            RamUse = ramUse;
            ProcessRamUse = processRamUse;
        }

        /// <summary>
        /// CPU使用率
        /// </summary>
        public double CpuRate { get; set; }
        /// <summary>
        /// 使用内存
        /// </summary>
        public double RamUse { get; set; }
        /// <summary>
        /// 进程使用内存
        /// </summary>
        public double ProcessRamUse { get; set; }


        public override string ToString()
        {
            return $"CpuRate:{CpuRate},RamUse:{RamUse},ProcessRamUse:{ProcessRamUse}";
        }
    }
}
