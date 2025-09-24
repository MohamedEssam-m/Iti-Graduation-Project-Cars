namespace Cars.BLL.Service.Implementation
{
    public class MechanicService : IMechanicService
    {
        private readonly IMechanicRepo repo;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public RoleManager<IdentityRole> RoleManager;
        public MechanicService(IMechanicRepo repo, IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.userManager = userManager;
            this.RoleManager = roleManager;
        }
        //CreateMechanicVM
        //UpdateMechanicVM
        public async Task Add(CreateMechanicVM mechanicVM)
        {
            try
            {
                var imagePath = Upload.UploadFile("Files", mechanicVM.Mechanic_Image);
                var mechanicMapped = new MechanicUser(
                    mechanicVM.FullName,
                    mechanicVM.Address,
                    mechanicVM.Specialization,
                    mechanicVM.ExperienceYears,
                    mechanicVM.WorkshopAddress,
                    imagePath
                );
                if (mechanicVM == null)
                    throw new ArgumentNullException("mechanicVM");
                else
                {
                    var mechanic = mapper.Map<MechanicUser>(mechanicVM);
                    repo.CreateMechanic(mechanic);
                    if (await RoleManager.RoleExistsAsync("Mechanic"))
                    {
                        userManager.AddToRoleAsync(mechanic, "Mechanic").Wait();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public void Delete(string id)
        {
            try
            {
                var mechanic = repo.GetMechanicById(id);
                if (mechanic != null && mechanic.Id != null)
                {
                    repo.DeleteMechanic(id);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("ID Not Found!!");
            }
        }

        public List<MechanicUser> GetAll()
        {
            try
            {
                List<MechanicUser> l = repo.GetAllMechanics();
                if (l == null)
                    throw new Exception();
                else
                    return l;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("There is no users to show");
            }
            return new List<MechanicUser>();
        }

        public MechanicUser GetById(string id)
        {
            try
            {
                var user = repo.GetMechanicById(id);
                if (user != null && user.Id != null)
                {
                    return repo.GetMechanicById(id);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("ID Not Found!!");
            }
            return new MechanicUser();
        }

        public void Update(UpdateMechanicVM mechanic)
        {
            try
            {
                if (mechanic != null)
                {
                    var mechanicuser = mapper.Map<MechanicUser>(mechanic);
                    repo.UpdateMechanic(mechanicuser);
                }
                else
                {
                    throw new Exception("user");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
