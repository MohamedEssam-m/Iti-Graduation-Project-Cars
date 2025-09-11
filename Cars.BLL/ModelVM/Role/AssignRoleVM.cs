using Cars.BLL.ModelVM.AppUserVM;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.ModelVM.Role
{
    public class AssignRoleVM
    {
        public List<UserWithRoleVM> UsersWithRole = new List<UserWithRoleVM>();
        public List<IdentityRole> RolesList = new List<IdentityRole>();
    }
}
