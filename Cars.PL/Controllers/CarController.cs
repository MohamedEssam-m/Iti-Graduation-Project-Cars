using AutoMapper;
using Cars.BLL.ModelVM.CarVM;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Repo.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var carVMs = _carService.GetAll();
                return View(carVMs);
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving cars.";
                return View(new List<CreateCarVM>());
            }
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(CreateCarVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _carService.Add(model);
                TempData["SuccessMessage"] = "Car created successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the car.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var car = _carService.GetById(id);
                if (car == null)
                {
                    TempData["ErrorMessage"] = "Car not found.";
                    return RedirectToAction("Index");
                }

                var carVM = new UpdateCarVM
                {
                    CarId = id,
                };

                return View(carVM);
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the car.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(UpdateCarVM momo)
        {
            if (!ModelState.IsValid)
                return View(momo);

            try
            {
                _carService.Update(momo);
                TempData["SuccessMessage"] = "Car updated successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the car.");
                return View(momo);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _carService.Delete(id);
                TempData["SuccessMessage"] = "Car deleted successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the car.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var car = _carService.GetById(id);
                if (car == null)
                {
                    TempData["ErrorMessage"] = "Car not found.";
                    return RedirectToAction("Index");
                }

                return View(car);
            }
            catch
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving car details.";
                return RedirectToAction("Index");
            }
        }
    }

}