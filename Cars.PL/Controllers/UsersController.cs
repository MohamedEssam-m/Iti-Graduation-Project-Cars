namespace Cars.PL.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAppUserService userService;
        private readonly IMapper mapper;

        private readonly UserManager<AppUser> UserManager;

        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly IRentService rentService;
        private readonly IAccidentService accidentService;
        private readonly IOfferService offerService;


        public UsersController(IRentService RentService, IOfferService offerService, IAccidentService accidentService, IAppUserService userService, IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userService = userService;
            this.mapper = mapper;
            UserManager = userManager;
            RoleManager = roleManager;
            this.rentService = RentService;
            this.accidentService = accidentService;
            this.offerService = offerService;
        }
        private async Task<List<UserWithRentCarsVM>> GetAllUsersWithRoles()
        {
            var users = await userService.GetAllUsers();
            var usersWithRoles = new List<UserWithRentCarsVM>();

            foreach (var user in users)
            {
                var roles = await UserManager.GetRolesAsync(user);
                if (user != null && user.Email != null && user.UserName != null && user.PhoneNumber != null)
                {
                    usersWithRoles.Add(new UserWithRentCarsVM
                    {
                        ImagePath = user.UserImagePath,
                        FullName = user.FullName,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        Age = user.Age,
                        id = user.Id,
                        Roleslist = roles?.ToList() ?? new List<string>(),
                        Carslist = user.Rents?.ToList() ?? new List<Rent>()
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
        public async Task<IActionResult> UpdateUserCarStatus()
        {
            return View(await GetAllUsersWithRoles() ?? new List<UserWithRentCarsVM>());
        }
        public async Task<IActionResult> AssignUserCarStatus(int RentId, string NewStatus)
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
            var rent = await rentService.GetRentById(RentId);
            if (rent != null)
            {
                rent.Status = NewStatus;
                var rentVM = mapper.Map<UpdateRentVM>(rent);
                await rentService.UpdateRent(rentVM);
                var allUsers = await GetAllUsersWithRoles();
                ViewBag.Success = "Rent status updated successfully!";
                return View("UpdateUserCarStatus", allUsers);
            }
            else
            {
                var allUsers = await GetAllUsersWithRoles();
                ViewBag.Error = "Rent Operation not found!";
                return View("UpdateUserCarStatus", allUsers);
            }
        }
        public async Task<IActionResult> AssignOfferStatusView()
        {
            var AllAccidents = await accidentService.GetAllAccidents();
            List<List<Offer>> Offerslist = new List<List<Offer>>();
            foreach (var accident in AllAccidents)
            {
                if (accident.Offers != null && accident.Offers.Any())
                {
                    var offers = await offerService.GetOffersByAccident(accident.AccidentId);
                    Offerslist.Add(offers);
                    //foreach (var offer in accident.Offers)
                    //{

                    //}
                }
            }
            return View(Offerslist);
        }
        public async Task<IActionResult> AssignOfferStatus(int OfferId, string NewStatus)
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


            //var rent = await rentService.GetRentById(RentId);
            //if (rent != null)
            //{
            //    rent.Status = NewStatus;
            //    var rentVM = mapper.Map<UpdateRentVM>(rent);
            //    await rentService.UpdateRent(rentVM);
            //    var allUsers = await GetAllUsersWithRoles();
            //    ViewBag.Success = "Car status updated successfully!";
            //    return View("UpdateUserCarStatus", allUsers);
            //}
            //else
            //{
            //    var allUsers = await GetAllUsersWithRoles();
            //    ViewBag.Error = "Rent Operation not found!";
            //    return View("UpdateUserCarStatus", allUsers);
            //}
            var offer = await offerService.GetOfferById(OfferId);
            if (offer != null)
            {
                offer.Status = NewStatus;
                var offerVM = mapper.Map<UpdateOfferVM>(offer);
                await offerService.UpdateOffer(offerVM);
                ViewBag.Success = "Offer status updated successfully!";
                var _AllAccidents = await accidentService.GetAllAccidents();
                List<List<Offer>> _Offerslist = new List<List<Offer>>();
                foreach (var accident in _AllAccidents)
                {
                    if (accident.Offers != null && accident.Offers.Any())
                    {
                        var offers = await offerService.GetOffersByAccident(accident.AccidentId);
                        _Offerslist.Add(offers);
                        //foreach (var offer in accident.Offers)
                        //{

                        //}
                    }
                }
                return View("AssignOfferStatusView", _Offerslist);
            }
            var allUsers = await GetAllUsersWithRoles();
            ViewBag.Error = "Offer not found!";
            var AllAccidents = await accidentService.GetAllAccidents();
            List<List<Offer>> Offerslist = new List<List<Offer>>();
            foreach (var accident in AllAccidents)
            {
                if (accident.Offers != null && accident.Offers.Any())
                {
                    var offers = await offerService.GetOffersByAccident(accident.AccidentId);
                    Offerslist.Add(offers);
                    //foreach (var offer in accident.Offers)
                    //{

                    //}

                    
                }
            }
            return View("AssignOfferStatusView", Offerslist);
        }
    }
}
