using System.Diagnostics;
using System.Threading.Tasks;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Accidents;
using Cars.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICarService CarService;
        private readonly IAccidentService accidentService;

        public HomeController(ILogger<HomeController> logger , ICarService carService, IAccidentService accidentService)
        {
            _logger = logger;
            CarService = carService;
            this.accidentService = accidentService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Home";
            var RandList = await CarService.GetAllRandom();
            return View(RandList.Take(3).ToList());
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
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            ViewData["ActivePage"] = "Admin Panel";
            return View();
        }
        public IActionResult ManageProfile()
        {
            ViewData["ActivePage"] = "ManageProfile";
            return View();
        }
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
