using Microsoft.AspNetCore.Mvc;

namespace SpinutechExercise.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
