﻿using Brun;
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
            _workerServer = WorkerServer.Instance;//或者构造函数中用 IWorkerServer 取
        }

        public IActionResult Index()
        {
            IList<IWorker> workers = WorkerServer.Instance.GetAllWorker();
            return View(workers);
        }

        public IActionResult Once()
        {
            //运行后台任务
            _workerServer.GetOnceWorker(Program.BrunKey).RunDontWait();
            return View();
        }
        public IActionResult LongOnce()
        {
            //运行后台任务
            (_workerServer.GetWokerByName(nameof(LongTimeBackRun)).First() as IOnceWorker).RunDontWait();
            return View();
        }
        public async Task<IActionResult> Queue(string msg)
        {
            //运行队列任务
            IQueueWorker worker = _workerServer.GetQueueWorker(Program.QueueKey);
            for (int i = 0; i < 100; i++)
            {
                //这里的await只是等待队列添加动作完成
                await worker.Enqueue(msg);
            }
            return View();
        }
        public IActionResult Scope()
        {
            //运行Scoped后台任务
            _workerServer.GetOnceWorker(Program.ScopeKey).RunDontWait();
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
