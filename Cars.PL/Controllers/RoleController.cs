namespace Cars.PL.Controllers
{
    
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IMapper mapper;

        private readonly IAppUserService AppUserService;

        private readonly UserManager<AppUser> UserManager;

        public RoleController(IRoleService roleService, IMapper mapper , IAppUserService appUserService , UserManager<AppUser> userManager)
        {
            this.roleService = roleService;
            this.mapper = mapper;
            AppUserService = appUserService;
            UserManager = userManager;
        }
        private async Task<AssignRoleVM> GetAllUsersWithRoles()
        {
            var users = await AppUserService.GetAllUsers();
            var Roles = await roleService.GetAllRoles();
            List<UserWithRoleVM> usersWithRoles = new List<UserWithRoleVM>();
            foreach (var user in users)
            {
                var roles = await UserManager.GetRolesAsync(user);
                usersWithRoles.Add(new UserWithRoleVM
                {
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Age = user.Age,
                    id = user.Id,
                    CreatedAt = (DateTime)user.CreatedAt,
                    Roleslist = (List<string>)roles

                });
            }
            var AssignRoleToUserVM = new AssignRoleVM
            {
                UsersWithRole = usersWithRoles,
                RolesList = Roles
            };

            return AssignRoleToUserVM;
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        public async Task<IActionResult> SaveRole(CreateRoleVM role)
        {
            if (ModelState.IsValid && role.Name != null)
            {
                await roleService.CreateRole(role);
                ViewBag.Success = "Role Created Successfully";
                return View("CreateRole");

            }
            else
            {
                ViewBag.Error = "SomeThing Is Wrong";
                return View("CreateRole", role);
            }

        }

        public async Task<IActionResult> UpdateRole()
        {
            var list = await roleService.GetAllRoles();
            var updateRoleVM = new UpdateRoleVM
            {
                RolesList = list
            };
            return View(updateRoleVM);

        }
        public async Task<IActionResult> SaveUpdateRole(UpdateRoleVM rolevm)
        {
            var allroles = await roleService.GetAllRoles();
            var role = allroles.FirstOrDefault(r => r.Name == rolevm.OldName);
            if (role != null && role.Id != null && role.Name != null)
            {
                var updateRoleVM = new UpdateRoleVM();
                var role1 = mapper.Map<UpdateRoleVM>(rolevm);
                await roleService.UpdateRole(role1);
                if(rolevm.NewName != null)
                { 
                    ViewBag.Success = "Role Updated Successfully To ";
                    updateRoleVM = new UpdateRoleVM
                    {
                        RolesList = await roleService.GetAllRoles()
                    };
                }

                //{
                //    RolesList = allroles
                //};
                return View("UpdateRole", updateRoleVM);
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("UpdateRole", rolevm);
        }

        public async Task<IActionResult> DeleteRole()
        {
            var list = await roleService.GetAllRoles();
            var deleteRoleVM = new DeleteRole
            {
                RolesList = list
            };
            return View(deleteRoleVM);
        }
        public async Task<IActionResult> SaveDeleteRole(DeleteRole rolevm)
        {
            var allroles = await roleService.GetAllRoles();
            var role = allroles.FirstOrDefault(r => r.Name == rolevm.Name);
            if (role != null && role.Id != null && role.Name != null)
            {
                await roleService.DeleteRole(role.Id);
                ViewBag.Success = "Role deleted successfully!";
                var deleteRoleVM = new DeleteRole
                {
                    RolesList = await roleService.GetAllRoles()
                };
                return View("DeleteRole", deleteRoleVM);
            }
            ViewBag.Error = "Invalid Name!";
            return View("DeleteRole", rolevm);

        }
        public async Task<IActionResult> Index()
        {
            var result = await roleService.GetAllRoles();
            return View(result);
        }
        public async Task<IActionResult> AssignRoleToUserView()
        {
            return View(await GetAllUsersWithRoles());
        }
        public async Task<IActionResult> AssignRoleToUser(string Id , string RoleName)
        {

            //var user = await AppUserService.GetById(Id);
            //if(ModelState.IsValid)
            //{
            //    if (user != null && !string.IsNullOrEmpty(RoleName))
            //    {
            //        await roleService.AssignRoleToUser(user, RoleName);
            //        ViewBag.Success = $"The Role {RoleName} Addes Successfilly";
            //        return View("AssignRoleToUserView" , users);
            //    }
            //}
            //ViewBag.Error = "Some Thing Was Wrong";
            //return View("AssignRoleToUserView", users);
            var AssignRoleToUserVM = await GetAllUsersWithRoles();
            if (string.IsNullOrEmpty(RoleName))
            {
                ViewBag.Error = "Please select a role";
                return View("AssignRoleToUserView", AssignRoleToUserVM);
            }

            var user = await AppUserService.GetById(Id);
            if (user == null)
            {
                ViewBag.Error = "User not found";
                return View("AssignRoleToUserView", AssignRoleToUserVM);
            }

            var result = await roleService.AssignRoleToUser(user, RoleName);
            if (result)
            {
                ViewBag.Success = $"Role '{RoleName}' assigned to {user.UserName} successfully";
                return View("AssignRoleToUserView", AssignRoleToUserVM);
            }
            else
            {
                ViewBag.Error = "Failed to assign role";
                return View("AssignRoleToUserView", AssignRoleToUserVM);
            }

            

        }

    }
}
