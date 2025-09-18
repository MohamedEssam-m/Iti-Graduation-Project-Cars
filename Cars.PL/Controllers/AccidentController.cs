using AutoMapper;
using Cars.BLL.ModelVM.Accident;
using Cars.BLL.Service.Abstraction;
using Cars.DAL.Entities.Accidents;
using Cars.DAL.Entities.Offers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cars.PL.Controllers
{
    public class AccidentController : Controller
    {

        private readonly IAccidentService accidentService;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IAppUserService appUserService;
        private readonly ICarService carService;
        private readonly IOfferService offerService;
        public AccidentController(IOfferService offerService,ICarService carService,IAppUserService appUserService,UserManager<AppUser> userManager,IMapper mapper,IAccidentService accidentService)
        {
            this.accidentService = accidentService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.appUserService = appUserService;
            this.carService = carService;
            this.offerService = offerService;
        }

        public async Task<IActionResult> Repair()
        {
            ViewData["ActivePage"] = "Repair";
            var AllAcidents = await accidentService.GetAllAccidents();
            return View(AllAcidents);
        }
        public async Task<IActionResult> Index()
        {
            var accidents = await accidentService.GetAllAccidents();
            return View(accidents);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //use appUserService instead of useing GetUserAsync to get the navigations of the user
            var user = await userManager.GetUserAsync(User);
            var UserId = user.Id;
            var userWithNavigations = await appUserService.GetById(UserId);
            List<Car> l = new List<Car>();
            if (userWithNavigations != null && userWithNavigations.Rents != null && userWithNavigations.Rents.Count > 0)
            {
                foreach (var rent in userWithNavigations.Rents)
                {
                    var car = await carService.GetById(rent.CarId);
                    l.Add(car);
                }
            }
            
            ViewBag.Cars = l;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAccidentVM accident)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                var IsTrue = await accidentService.AddAccident(accident, user.Id);
                if (IsTrue)
                {
                    var AllAccidents = await accidentService.GetAllAccidents();
                    ViewBag.Success = "The Accident Created Successfully";
                    return View("Repair" , AllAccidents);
                }
                
            }
            ViewBag.Error = "Some Thing Was Wrong!";
            var user1 = await userManager.GetUserAsync(User);
            var UserId = user1.Id;
            var userWithNavigations = await appUserService.GetById(UserId);
            return View("Create", userWithNavigations);
        }
        [HttpGet]
        public async Task<IActionResult> EditView(int accidentId)
        {
            var accident = await accidentService.GetAccidentById(accidentId);
            if (accident == null)
            {
                ViewBag.Error = "Some Thing Was Wrong";
                var mainAccident = mapper.Map<UpdateAccidentVM>(accident);
                var listOfAccidents = await accidentService.GetAllAccidents();
                return View("Repair" , listOfAccidents);
            }
            //use appUserService instead of useing GetUserAsync to get the navigations of the user
            var user = await userManager.GetUserAsync(User);
            var UserId = user.Id;
            var userWithNavigations = await appUserService.GetById(UserId);
            List<Car> l = new List<Car>();
            if (userWithNavigations != null && userWithNavigations.Rents != null && userWithNavigations.Rents.Count > 0)
            {
                foreach (var rent in userWithNavigations.Rents)
                {
                    var car = await carService.GetById(rent.CarId);
                    l.Add(car);
                }
            }

            ViewBag.Cars = l;
            var Accident = mapper.Map<UpdateAccidentVM>(accident);
            return View(Accident);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAccidentVM accident)
        {
            if (ModelState.IsValid)
            {
                var IsTrue = await accidentService.UpdateAccident(accident);
                if(IsTrue)
                {
                    ViewBag.Success = "The Accident Updated Successfully";
                    return View("EditView" , accident);
                }
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("EditView", accident);
        }
        public async Task<IActionResult> DeleteView(int accidentId)
        {
            var accident = await accidentService.GetAccidentById(accidentId);
            if (accident == null)
            {
                ViewBag.Error = "Some Thing Was Wrong";
                var mainAccident = mapper.Map<UpdateAccidentVM>(accident);
                var listOfAccidents = await accidentService.GetAllAccidents();
                return View("Repair");
            }
            return View(accident);
            
        }
        public async Task<IActionResult> Delete(int accidentId)
        {
            var IsTrue = await accidentService.DeleteAccident(accidentId);
            if (IsTrue)
            {
                ViewBag.Success = "The Accident Deleted Successfully";
                return View("DeleteView");
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("DeleteView");
        }
        public async Task<IActionResult> Offers(int accidentId , int CarId)
        {
            var car = await carService.GetById(CarId);
            var AllOffers = await offerService.GetOffersByAccident(accidentId);
            foreach (var offer in AllOffers)
            {
                offer.CarName = car.Brand;
            }
            var accident = await accidentService.GetAccidentById(accidentId);
            var user = await appUserService.GetById(accident.UserId);
            ViewBag.USERID = user.Id;
            return View(AllOffers);
        }
        //public async Task<IActionResult> Details(int id)
        //{
        //    var accident = await accidentService.GetAccidentById(id);
        //    if (accident == null)
        //        return NotFound();

        //    return View(accident);
        //}
    }
}
