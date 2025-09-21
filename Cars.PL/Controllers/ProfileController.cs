using AutoMapper;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Cars.DAL.Entities.Accidents;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cars.PL.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IAppUserService userService;
        private readonly ICarService carService;
        private readonly IRentService rentService;
        private readonly IMapper mapper;
        private readonly IAccidentService accidentService;

        public ProfileController(IAccidentService accidentService,UserManager<AppUser> userManager, IAppUserService userService, ICarService carService, IRentService rentService, IMapper mapper)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.carService = carService;
            this.rentService = rentService;
            this.mapper = mapper;
            this.accidentService = accidentService;
        }
        //profile view
        public async Task<IActionResult> ProfileView()
        {
            var usermanager = await userManager.GetUserAsync(User);
            var UserWithNavigations = await userService.GetById(usermanager.Id);
            var AllAcidents = await accidentService.GetAllAccidents();
            
            List<Accident> UserAccidents = new List<Accident>();
            foreach (var accident in AllAcidents)
            {
                if(accident.UserId == UserWithNavigations.Id)
                {
                    UserAccidents.Add(accident);
                }
            }
            ViewBag.ACCIDENTS = UserAccidents;
            return View(UserWithNavigations);
        }
    }
}
