using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class CarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
