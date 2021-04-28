using Brun;
using Brun.Commons;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunConsoleSimple
{
    class Program
    {
        static string GetNow => DateTime.Now.ToString("HH:mm:ss ffff");
        static async Task DoAsync()
        {
            Console.WriteLine(GetNow + "  DoAsync  开始");
            await Task.Delay(TimeSpan.FromSeconds(3));
            //Task t = Task.Delay(TimeSpan.FromSeconds(3));
            Console.WriteLine(GetNow + "  DoAsync  结束");
            //return t;
        }
        static async Task DoTask()
        {
            Console.WriteLine(GetNow + "  DoTask  开始");
            await DoAsync();
            Console.WriteLine(GetNow + "  DoTask  结束");
            //return t;
            //return Task.CompletedTask;
        }
        static Task Do()
        {
            Console.WriteLine(GetNow + "  Do  开始");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine(GetNow + "  Do  结束");
            return Task.CompletedTask;
        }
        static async Task Main(string[] args)
        {
            //Thread.Sleep(TimeSpan.FromSeconds(5));

            await DoAsync();
            await Do();
            await DoTask();
            //await Task.Delay(TimeSpan.FromSeconds(2));
            Console.WriteLine(GetNow + "  Task All Over");
            await Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task2 线程");
            });
            Console.ReadKey();
            //Console.WriteLine("进程名称："+Process.GetCurrentProcess().ProcessName);
            //GetSystemInfo();

            //int i = 0;
            //while (i < 10000)
            //{
            //    i++;
            //    //GetRamInfo();
            //    //BaseInfo();
            //    UsingProcess();
            //    Random random = new Random();
            //    for (int n = 0; n < 100; n++)
            //    {
            //        var ran = random.Next();
            //    }

            //    //long working = Process.GetCurrentProcess().WorkingSet64;
            //    //var process = Process.GetCurrentProcess();
            //    ////IntPtr MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
            //    //Console.WriteLine("i:{0},{1:f}m,startTime:{2},Threads:{3}", i, working / 1024f / 1024f, process.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), process.Threads.Count);
            //    Thread.Sleep(TimeSpan.FromSeconds(1));
            //}

            //IHost host = CreateHostBuilder(args).Build();
            //host.Run();
        }
        static void UsingProcess()
        {
            //Console.WriteLine("所有进程TotalProcessorTime:" + Process.GetProcesses().Sum(m=>m.UserProcessorTime.TotalMilliseconds));

            Console.WriteLine("TotalProcessorTime:" + Process.GetCurrentProcess().TotalProcessorTime);
            Console.WriteLine("CPU使用率" + Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds / 1000 * 100 / 8);
            Console.WriteLine("进程占用Cpu时间" + Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds);
            var TotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime.TotalMilliseconds / TimeSpan.FromSeconds(1).TotalMilliseconds / Environment.ProcessorCount * 100;
            var PrivilegedProcessorTime = Process.GetCurrentProcess().PrivilegedProcessorTime.TotalMilliseconds / TimeSpan.FromSeconds(1).TotalMilliseconds / Environment.ProcessorCount * 100;
            var UserProcessorTime = Process.GetCurrentProcess().UserProcessorTime.TotalMilliseconds / TimeSpan.FromSeconds(1).TotalMilliseconds / Environment.ProcessorCount * 100;


            Console.WriteLine(TotalProcessorTime);
            Console.WriteLine(PrivilegedProcessorTime);
            Console.WriteLine(UserProcessorTime);

        }
        private static void BaseInfo()
        {
            // Define variables to track the peak
            // memory usage of the process.
            long peakPagedMem = 0,
                 peakWorkingSet = 0,
                 peakVirtualMem = 0;

            // Start the process.
            using (Process myProcess = Process.Start("NotePad.exe"))
            {
                // Display the process statistics until
                // the user closes the program.
                do
                {
                    if (!myProcess.HasExited)
                    {
                        // Refresh the current process property values.
                        myProcess.Refresh();

                        Console.WriteLine();

                        // Display current process statistics.

                        Console.WriteLine($"{myProcess} -");
                        Console.WriteLine("-------------------------------------");

                        Console.WriteLine($"  Physical memory usage     : {myProcess.WorkingSet64}");
                        Console.WriteLine($"  Base priority             : {myProcess.BasePriority}");
                        Console.WriteLine($"  Priority class            : {myProcess.PriorityClass}");
                        Console.WriteLine($"  User processor time       : {myProcess.UserProcessorTime}");
                        Console.WriteLine($"  Privileged processor time : {myProcess.PrivilegedProcessorTime}");
                        Console.WriteLine($"  Total processor time      : {myProcess.TotalProcessorTime}");
                        Console.WriteLine($"  Paged system memory size  : {myProcess.PagedSystemMemorySize64}");
                        Console.WriteLine($"  Paged memory size         : {myProcess.PagedMemorySize64}");

                        // Update the values for the overall peak memory statistics.
                        peakPagedMem = myProcess.PeakPagedMemorySize64;
                        peakVirtualMem = myProcess.PeakVirtualMemorySize64;
                        peakWorkingSet = myProcess.PeakWorkingSet64;

                        if (myProcess.Responding)
                        {
                            Console.WriteLine("Status = Running");
                        }
                        else
                        {
                            Console.WriteLine("Status = Not Responding");
                        }
                    }
                }
                while (!myProcess.WaitForExit(1000));

                Console.WriteLine();
                Console.WriteLine($"  Process exit code          : {myProcess.ExitCode}");

                // Display peak memory statistics for the process.
                Console.WriteLine($"  Peak physical memory usage : {peakWorkingSet}");
                Console.WriteLine($"  Peak paged memory usage    : {peakPagedMem}");
                Console.WriteLine($"  Peak virtual memory usage  : {peakVirtualMem}");
            }
        }
        public static void GetSystemInfo()
        {
            var info = MachineUtil.GetMachineBaseInfo(0);
            Console.WriteLine(info.ToString());
        }
        public static void GetRamInfo()
        {
            var info = MachineUtil.GetMachineUseInfo();
            Console.WriteLine(info.ToString());
        }
        //TODO PerformanceCounter cpu监控
        //
        public static void ProcessTest()
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe");
                //process.StartInfo.Arguments = "pwd";
                //process.StartInfo.Arguments = "dotnet --info";
                process.StartInfo.Arguments = "ping www.baidu.com -t";
                //process.Start();

                //process.StartInfo.Arguments = "dotnet run -p D:\\work\\Brun\\simples\\BrunWebTest\\BrunWebTest.csproj";
                //process.OutputDataReceived += Process_OutputDataReceived;
                //process.ErrorDataReceived += Process_ErrorDataReceived;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.StandardInputEncoding = Encoding.UTF8;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                Task.Factory.StartNew(() =>
                {
                    using (System.IO.StreamReader sr = process.StandardOutput)
                    {
                        while (!sr.EndOfStream)
                        {
                            Console.WriteLine("-----------date--------------");
                            string str = sr.ReadLine();
                            Console.WriteLine(str);

                        }
                        sr.Close();
                    }
                });
                Thread.Sleep(TimeSpan.FromSeconds(5));
                //process.Close();
                process.Kill();
                //process.WaitForExit();
                //process.StandardInput.WriteLine("exit");
                //process.StandardInput.Flush();
                //process.BeginOutputReadLine();
                //process.WaitForExit();
            }

            //using (var process = Process.Start("C:\\Windows\\System32\\WindowsPowerShell\\v1.0\\powershell.exe", "dotnet --info"))
            //{
            //    process.OutputDataReceived += Process_OutputDataReceived;
            //    process.ErrorDataReceived += Process_ErrorDataReceived;
            //}
            //Console.Read();
        }
        private static void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("----------error-----------");
            //Console.WriteLine(e.Data);
        }

        private static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("----------data-----------");
            //Console.WriteLine(e.Data);
        }

        /*
框架提供的服务
自动注册以下服务：
   IHostApplicationLifetime
   IHostLifetime
   IHostEnvironment / IWebHostEnvironment
*/
        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
            })
            .ConfigureHostConfiguration(m =>
            {
            })
            .ConfigureServices((hostContext, services) =>
            {
                //services.AddLogging(logConfigure =>
                //{

                //});
                services.AddBrunService();
                //services.AddHostedService<BrunBackgroundService>();
            });
    }
}
