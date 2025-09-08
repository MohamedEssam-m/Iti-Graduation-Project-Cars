using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.ModelVM.Role;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IAppUserService
    {
        Task<bool> Add(CreateUserVM user);
        Task<List<AppUser>> GetAllUsers();
        Task<bool> DeleteUser(string roleId);
        Task<bool> UpdateUser(string id, UpdateUserVM roleVM);
        Task<AppUser> GetById(string id);
    }
}
