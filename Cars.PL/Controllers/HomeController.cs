using System.Diagnostics;
using Cars.BLL.Service.Abstraction;
using Cars.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICarService CarService;

        public HomeController(ILogger<HomeController> logger , ICarService carService)
        {
            _logger = logger;
            CarService = carService;
        }

        public IActionResult Index()
        {
            ViewData["ActivePage"] = "Home";
            return View();
        }
        public IActionResult About()
        {
            ViewData["ActivePage"] = "About";
            return View();
        }
        public async Task<IActionResult> Cars()
        {
            ViewData["ActivePage"] = "Cars";
            var cars =  await CarService.GetAll();
            return View(cars);
        }
        public IActionResult AdminPanel()
        {
            ViewData["ActivePage"] = "Admin Panel";
            return View();
        }
        //public IActionResult ManageProfile()
        //{
        //    ViewData["ActivePage"] = "ManageProfile";
        //    return View();
        //}
        public IActionResult Contact()
        {
            ViewData["ActivePage"] = "Contact";
            return View();
        }
        public IActionResult Services()
        {
            ViewData["ActivePage"] = "Services";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["ActivePage"] = "Privacy";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
