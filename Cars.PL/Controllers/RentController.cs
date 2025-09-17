using Cars.BLL.Helper;
using Cars.BLL.ModelVM.Payment;
using Cars.BLL.ModelVM.Rent;
using Cars.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class RentController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ICarService carService;
        private readonly PayPal Paypal;
        
        public RentController(UserManager<AppUser> user, IConfiguration Configuration , ICarService carService)
        {
            this.userManager = user;
            this.carService = carService;
            Paypal = new PayPal
            {
                PayPalClientId = Configuration["PayPal:ClientId"],
                PayPalSecret = Configuration["PayPal:ClientSecret"],
                PayPalUrl = Configuration["PayPal:URL"]
            };
        }
        public async Task<IActionResult> RentingDetails(int CarId)
        {
            var cars = await carService.GetAll();
            var car =  await carService.GetById(CarId);
            if(car.quantity < 1)
            {
                ViewBag.Error = $"{car.Brand} {car.Model} {car.Year} is not Available Right Now";
                return View("Cars", cars);
            }
            var userId = userManager.GetUserId(User);
            if(userId == null)
            {
                ViewBag.Error = "You must be logged in to rent a car";
                return View("Cars", cars);
            }
            var CreateRentVM = new CreateRentVM()
            {
                CarId = CarId,
                UserId = userId,
            };
            return View(CreateRentVM);
        }
        //public async Task<IActionResult> Create()
        //{

        //    return View();
        //}
        

    }
}
