using AutoMapper;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.Role;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly IAppUserService appUserService;
        private readonly IMapper mapper;
        public UserController(IAppUserService appUserService , IMapper mapper)
        {
            this.appUserService = appUserService;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var list = appUserService.GetAllUsers();
            return View(list);
        }
        public IActionResult CreateUser()
        {
            return View();
        }
        public async Task<IActionResult> SaveUser(CreateUserVM user)
        {
            if (ModelState.IsValid)
            {
                await appUserService.Add(user);
                ViewBag.Success = "User Created Successfully";
                return View("CreateUser" , user);
            }
            else
            {
                ViewBag.Error = "SomeThing Is Wrong";
                return View("CreateUser", user);
            }

        }
        
    }
}
