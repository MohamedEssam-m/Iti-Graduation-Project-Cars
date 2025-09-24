namespace Cars.BLL.Service.Abstraction
{
    public interface IAppUserService
    {
        Task<bool> Add(CreateUserVM user);
        Task<List<AppUser>> GetAllUsers();
        Task<bool> DeleteUser(string roleId);
        Task<bool> UpdateUser(UpdateUserVM roleVM);
        Task<AppUser> GetById(string id);
    }
}
