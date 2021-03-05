using Brun;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrunWebTest.Controllers
{
    public class WorkerController : Controller
    {
        public IActionResult Index()
        {
            IList<IWorker> workers= WorkerServer.Instance.GetAllWorker();
            
            return View(workers);
        }
    }
}
