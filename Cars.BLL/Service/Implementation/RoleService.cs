using AutoMapper;
using Cars.BLL.ModelVM.Role;
using Cars.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;
        public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
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
            }
            catch (Exception)
            {

                return false;
            }
            return false;
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
    }
}
