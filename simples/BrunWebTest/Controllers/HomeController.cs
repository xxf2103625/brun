using Brun;
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
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _workerServer = WorkerServer.Instance;
        }

        public IActionResult Index()
        {
            return View();
        }

        public  IActionResult Privacy()
        {
            //运行后台任务
            _workerServer.GetWorker(Program.BrunKey).RunDontWait();
            return View();
        }
        public async Task<IActionResult> Queue(string msg)
        {
            IQueueWorker worker=_workerServer.GetQueueWorker(Program.QueueKey);
            for (int i = 0; i < 100; i++)
            {
                await worker.Enqueue(msg);
            }
            return Content("Queue test");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
