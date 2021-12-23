using Brun.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrunSimple.Controllers
{
    public class BrunController : Controller
    {
        IOnceBrunService onceWorker;
        public BrunController(IOnceBrunService onceWorker)
        {
            this.onceWorker = onceWorker;
        }
        public IActionResult Index()
        {
            //onceWorker.AddOnceBrun(new Brun.Models.WorkerConfigModel() { Key = "t1", Name = "tName" });
            return Content("Index");
        }
    }
}
