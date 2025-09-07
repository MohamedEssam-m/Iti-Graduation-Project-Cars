using Cars.BLL.ModelVM.Role;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IRoleService
    {
        Task<bool>  CreateRole(CreateRoleVM roleVM); 
        Task<List<IdentityRole>> GetAllRoles();
        Task<bool> DeleteRole(string roleId);
        Task<bool> UpdateRole(UpdateRoleVM roleVM);
    }
}
