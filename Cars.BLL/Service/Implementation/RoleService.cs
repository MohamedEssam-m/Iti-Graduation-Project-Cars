namespace Cars.BLL.Service.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        private readonly UserManager<AppUser> UserManager;

        public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper , UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
            UserManager = userManager;
        }
        public async Task<bool> CreateRole(CreateRoleVM roleVM)
        {
            try
            {
                var getRoleByName = await roleManager.FindByNameAsync(roleVM.Name);
                if (getRoleByName is not { })
                {
                    var role = mapper.Map<IdentityRole>(roleVM);
                    var result = await roleManager.CreateAsync(role);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
            
        }

        public async Task<bool> DeleteRole(string roleId)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(roleId);
                if (role == null || role.Id == null || role.Name == null) return false;

                var result = await roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            try
            {
                var roles = roleManager.Roles.ToList();
                return roles;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<IdentityRole>();
            }
        }

        public async Task<bool> UpdateRole(UpdateRoleVM roleVM)
        {
            try
            {
                
                var role = await roleManager.FindByNameAsync(roleVM.OldName);
                if (role == null || role.Name == null || role.Id == null) return false;

                role.Name = roleVM.NewName;
                var result = await roleManager.UpdateAsync(role);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> AssignRoleToUser(AppUser user, string roleName)
        {
            try
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null)
                    return false;

                
                if (await UserManager.IsInRoleAsync(user, roleName))
                    return false;

                var result = await UserManager.AddToRoleAsync(user, roleName);
                if(result.Succeeded) 
                    return true;
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
