using Microsoft.AspNetCore.Mvc;

namespace BrunSimple.Controllers
{
    public class BrController: Controller
    {
        public IActionResult Index()
        {
            return Content("Index");
        }

    }
}
