using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
