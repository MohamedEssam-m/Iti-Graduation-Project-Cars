using AutoMapper;
using Cars.BLL.ModelVM.CarVM;
using Cars.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cars.PL.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarService carService;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        public CarsController(ICarService carService , IMapper mapper , UserManager<AppUser> userManager)
        {
            this.carService = carService;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await carService.GetAll();
            return View(cars);
        }
        public IActionResult CreateCarView()
        {
            return View();
        }
        public async Task<IActionResult> CreateCar(CreateCarVM CarVM)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid Data!!";
                return View("CreateCarView", CarVM);
            }
            
            var result = await carService.Add(CarVM);
            if (result)
            {
                ViewBag.Success = "Car Created Successfully";
                return View("CreateCarView");
            }
            ViewBag.Error = "Invalid Data!!";
            return View("CreateCarView", CarVM);


        }
        public async Task<IActionResult> UpdateCarView(int carId)
        {
            var car = await carService.GetById(carId);
            var carVM = mapper.Map<UpdateCarVM>(car);
            carVM.ImagePath = car.CarImagePath;
            return View(carVM);
        }
        public async Task<IActionResult> UpdateCar(UpdateCarVM carVM)
        {
            if(ModelState.IsValid)
            {
                var result = await carService.Update(carVM);
                if (result)
                {
                    ViewBag.Success = "Car Updated Successfully";
                    return View("UpdateCarView", carVM);
                }
            }
            ViewBag.Error = "Error while updating Car";
            return View("UpdateCarView", carVM);
        }
        public async Task<IActionResult> DeleteCarView(int carId)
        {
            var car = await carService.GetById(carId);
            return View(car);
        }
        public async Task<IActionResult> DeleteCar(int carId)
        {
            
            var car  = await carService.GetById(carId);
            var result = await carService.Delete(carId);
            if (result)
            {
                var Cars = await carService.GetAll();
                ViewBag.Success = "Car Deleted Successfully";
                return View("GetAllCars", Cars);
            }
            var cars = await carService.GetAll();
            ViewBag.Error = "Error while deleting Car";
            return View("GetAllCars", cars);
        }
        public async Task<IActionResult> CarDetails(int carId)
        {
            var car = await carService.GetById(carId);
            return View(car);
        }
        //public async Task<IActionResult> CarRate(int carId)
        //{
        //    var car = await carService.GetById(carId);
        //    return View(car);
        //}
        public async Task<IActionResult> SaveCareRate(RateCarVM rateCar)
        {
            var UserId = userManager.GetUserId(User);
            if (UserId == null)
            {
                ViewBag.Error = "Please , LogIn to rate a car";
                var car = await carService.GetById(rateCar.CarId);
                return View("CarDetails", car);
            }
            if (ModelState.IsValid)
            {
                var car = await carService.GetById(rateCar.CarId);
                var result = carService.RateCar(rateCar , UserId);
                if (result.Result)
                {
                    ViewBag.Success = "Rate Saved Successfully";
                    return View("CarDetails", car);
                }
            }
            ViewBag.Error = "Some Thing Was Wrong!";
            var Car = await carService.GetById(rateCar.CarId);
            return View("CarDetails", Car);
        }
        public async Task<IActionResult> ViewRatings(int CarId)
        {
            var car = await carService.GetById(CarId);
            return View(car);
        }
    }
}
