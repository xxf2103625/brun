using Brun;
using BrunSimple.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BrunSimple.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        WorkerServer _workerServer;
        public HomeController(ILogger<HomeController> logger,WorkerServer workerServer)
        {
            _logger = logger;
            _workerServer = workerServer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            //_workerServer.GetOnceWorker("t1").Run(typeof(BrunTestHelper.BackRuns.LogBackRun));
            //_workerServer.GetOnceWorker("t1").Run();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}