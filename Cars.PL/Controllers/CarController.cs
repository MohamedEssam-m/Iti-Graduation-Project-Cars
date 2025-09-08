using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Cars.DAL.Repo.Abstraction;
using Cars.BLL.ModelVM.CarVM;

namespace Cars.PL.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly ICarRepo _carRepo;
        private readonly IMapper _mapper;

        public CarController(ICarRepo carRepo, IMapper mapper)
        {
            _carRepo = carRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var cars = _carRepo.GetAll();
                var carVMs = _mapper.Map<List< CreateCarVM>> (cars);
                return View(carVMs);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving cars.";
                return View(new List<CreateCarVM>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCarVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var car = _mapper.Map<Car>(model);
                _carRepo.Add(car);

                TempData["SuccessMessage"] = "Car created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
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
                var car = _carRepo.GetById(id);
                if (car == null || car.CarId == 0)
                {
                    TempData["ErrorMessage"] = "Car not found.";
                    return RedirectToAction("Index");
                }

                var carVM = _mapper.Map<UpdateCarVM>(car);
                return View(carVM);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving the car.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdateCarVM model)
        {
            if (id != model.CarId)
            {
                TempData["ErrorMessage"] = "Car ID mismatch.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var existingCar = _carRepo.GetById(id);
                if (existingCar == null || existingCar.CarId == 0)
                {
                    TempData["ErrorMessage"] = "Car not found.";
                    return RedirectToAction("Index");
                }
                _mapper.Map(model, existingCar);
                _carRepo.Update(existingCar);

                TempData["SuccessMessage"] = "Car updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the car.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var car = _carRepo.GetById(id);
                if (car == null || car.CarId == 0)
                {
                    TempData["ErrorMessage"] = "Car not found.";
                    return RedirectToAction("Index");
                }

                _carRepo.Delete(id);
                TempData["SuccessMessage"] = "Car deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
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
                var car = _carRepo.GetById(id);
                if (car == null || car.CarId == 0)
                {
                    TempData["ErrorMessage"] = "Car not found.";
                    return RedirectToAction("Index");
                }

                var carVM = _mapper.Map<CreateCarVM>(car);
                return View(carVM);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while retrieving car details.";
                return RedirectToAction("Index");
            }
        }
    }
}