using AutoMapper;
using Cars.BLL.ModelVM.AppUserVM;

using Cars.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Cars.PL.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAppUserService userService;
        private readonly IMapper mapper;

        private readonly UserManager<AppUser> UserManager;

        private  RoleManager<IdentityRole> RoleManager;

        public UsersController(IAppUserService userService, IMapper mapper, UserManager<AppUser> userManager ,RoleManager <IdentityRole> roleManager)
        {
            this.userService = userService;
            this.mapper = mapper;
            UserManager = userManager;
            RoleManager = roleManager;
        }
        private async Task<List<UserWithRoleVM>> GetAllUsersWithRoles()
        {
            var users = await userService.GetAllUsers();
            var usersWithRoles = new List<UserWithRoleVM>();

            foreach (var user in users)
            {
                var roles = await UserManager.GetRolesAsync(user);
                if (user != null && user.Email != null && user.UserName != null && user.PhoneNumber != null)
                {
                    usersWithRoles.Add(new UserWithRoleVM
                    {
                        FullName = user.FullName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        Age = user.Age,
                        id = user.Id,
                        Roleslist = (List<string>)roles
                    });
                }
            }
            return usersWithRoles;
        }

        public async Task<IActionResult> GetUsers()
        {
            return View(await GetAllUsersWithRoles());
        }
        public IActionResult CreateUserView()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM user)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = await userService.Add(user);
                if (isCreated)
                {
                    ViewBag.Success = "User created successfully.";
                    return View("CreateUserView");
                }
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("CreateUserView", user);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUserView()
        {
            var usersWithRoles = await GetAllUsersWithRoles();

        
            return View(usersWithRoles);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (id == null)
            {
                ViewBag.Error = "Some Thing Was Wrong";
                return View("DeleteUserView", await GetAllUsersWithRoles());
            }
            bool isDeleted = await userService.DeleteUser(id);
            if (isDeleted)
            {
                ViewBag.Success = "User Deleted successfully";
                return View("DeleteUserView", await GetAllUsersWithRoles());
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("DeleteUserView", await GetAllUsersWithRoles());
        }
    }
}
