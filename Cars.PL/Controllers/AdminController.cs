namespace Cars.PL.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAppUserService userService;
        private readonly IMapper mapper;

        private readonly UserManager<AppUser> UserManager;

        private RoleManager<IdentityRole> RoleManager;
        public AdminController(IAppUserService userService, IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userService = userService;
            this.mapper = mapper;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public async Task<List<UserWithRoleVM>> GetAllUsersWithRoles()
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
        public async Task<IActionResult> Index()
        {
            var list = await GetAllUsersWithRoles();
            var AdminList = new List<UserWithRoleVM>();
            foreach (var user in list)
            {
                foreach (var role in user.Roleslist)
                {
                    if (role == "Admin")
                    {
                        AdminList.Add(user);
                        break;
                    }
                    
                }
            }
            return View(AdminList);
        }
        public IActionResult CreateAdminView(string role)
        {
            SignUpVM signUp = new SignUpVM();
            signUp.Role = role;
            return View(signUp);
        }
    }
}
