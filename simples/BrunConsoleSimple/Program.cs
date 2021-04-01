using Brun;
using Brun.Commons;
using BrunTestHelper.BackRuns;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrunConsoleSimple
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            while (i<10000)
            {
                i++;
                long working = Process.GetCurrentProcess().WorkingSet64;
                IntPtr MaxWorkingSet = Process.GetCurrentProcess().MaxWorkingSet;
                Console.WriteLine("i:{0},{1}m, max:{1}",i,working/1024/1024,MaxWorkingSet);
                Thread.Sleep(5);
            }
            
            //IHost host = CreateHostBuilder(args).Build();
            //host.Run();
        }
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
