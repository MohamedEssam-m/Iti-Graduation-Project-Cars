using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
