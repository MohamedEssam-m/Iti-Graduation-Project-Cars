namespace Cars.BLL.Service.Abstraction
{
    public interface IRoleService
    {
        Task<bool>  CreateRole(CreateRoleVM roleVM); 
        Task<List<IdentityRole>> GetAllRoles();
        Task<bool> DeleteRole(string roleId);
        Task<bool> UpdateRole(UpdateRoleVM roleVM);
        Task<bool> AssignRoleToUser(AppUser user, string roleName);
    }
}
