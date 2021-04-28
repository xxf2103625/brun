using System;
using System.Collections.Generic;
using System.Text;

namespace Brun.Models
{
    public class SystemInfo
    {
        public SystemInfo()
        {
        }

        public SystemInfo(string hostName, string systemOs, string osArchitecture, string dotnetDescription, int processorCount, double totalRam)
        {
            HostName = hostName;
            SystemOs = systemOs;
            OsArchitecture = osArchitecture;
            DotnetDescription = dotnetDescription;
            ProcessorCount = processorCount;
            TotalRam = totalRam;
        }

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
        /// 总内存 单位GB
        /// </summary>
        public double TotalRam { get; set; }
    }
    /// <summary>
    /// 系统资源实时信息
    /// </summary>
    public class SystemRunInfo
    {
        /// <summary>
        /// 时间点，精确到秒
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Cpu使用率
        /// </summary>
        public double CpuRage { get; set; }
        /// <summary>
        /// 内存使用，单位mb
        /// </summary>
        public double RamUse { get; set; }
        /// <summary>
        /// 当前进程内存使用，单位mb
        /// </summary>
        public double ProcessRamUse { get; set; }
        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
