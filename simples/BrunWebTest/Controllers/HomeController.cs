using Brun;
using Brun.Services;
using BrunWebTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BrunWebTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Brun.IWorkerServer _workerServer;
        BrunMonitor _brunMonitor;
        public HomeController(ILogger<HomeController> logger,BrunMonitor brunMonitor)
        {
            _logger = logger;
            _workerServer = WorkerServer.Instance;//或者构造函数中用 IWorkerServer 取
            _brunMonitor = brunMonitor;
        }

        public IActionResult Index()
        {
            var model = _brunMonitor.GetBrunInfo();
            return View(model);
        }

        public IActionResult Once()
        {
            //运行后台任务
            _workerServer.GetOnceWorker(Program.BrunKey).Run();
            return View();
        }
        public IActionResult LongOnce()
        {
            //运行后台任务
            (_workerServer.GetWokerByName(nameof(LongTimeBackRun)).First() as IOnceWorker).Run();
            return View();
        }
        public IActionResult Queue(string msg)
        {
            //运行队列任务
            IQueueWorker worker = _workerServer.GetQueueWorker(Program.QueueKey);
            for (int i = 0; i < 100; i++)
            {
                worker.Enqueue(msg);
            }
            return View();
        }
        public IActionResult Scope()
        {
            //运行Scoped后台任务
            _workerServer.GetOnceWorker(Program.ScopeKey).Run();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
