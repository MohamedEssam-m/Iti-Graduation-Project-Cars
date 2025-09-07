using AutoMapper;
using Cars.BLL.ModelVM.Role;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cars.PL.Controllers
{
    //[Authorize("Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;
        private readonly IMapper mapper;

        public RoleController(IRoleService roleService , IMapper mapper)
        {
            this.roleService = roleService;
            this.mapper = mapper;
        }
        public  IActionResult CreateRole()
        {
            return View();
        }
        public async Task<IActionResult> SaveRole(CreateRoleVM role)
        {
            if(ModelState.IsValid && role.Name != null)
            {
                await roleService.CreateRole(role);
                ViewBag.Success = "Role Created Successfully";
                return View("CreateRole");

            }
            else
            {
                ViewBag.Error = "SomeThing Is Wrong";
                return View("CreateRole" , role);
            }
                
        }

        public async Task<IActionResult> UpdateRole()
        {
            var list = await roleService.GetAllRoles();
            var updateRoleVM = new UpdateRoleVM
            {
                RolesList = list
            };
            return View(updateRoleVM);
            
        }
        public async Task<IActionResult> SaveUpdateRole(UpdateRoleVM rolevm)
        {
            var allroles = await roleService.GetAllRoles();
            var role = allroles.FirstOrDefault(r => r.Name == rolevm.OldName);
            if (role != null && role.Id != null && role.Name != null)
            {
                var role1 = mapper.Map<UpdateRoleVM>(rolevm);
                await roleService.UpdateRole(role1);
                ViewBag.Success = "Role Updated Successfully";
                var updateRoleVM = new UpdateRoleVM
                {
                    RolesList = await roleService.GetAllRoles()
                };
                
                //{
                //    RolesList = allroles
                //};
                return View("UpdateRole" , updateRoleVM);
            }
            ViewBag.Error = "Invalid Id";
            return View("UpdateRole" , rolevm);
        }

        public async  Task<IActionResult> DeleteRole()
        {
            var list = await roleService.GetAllRoles();
            var deleteRoleVM = new DeleteRole
            {
                RolesList = list
            };
            return View(deleteRoleVM);
        }
        public async Task<IActionResult> SaveDeleteRole(DeleteRole rolevm)
        {
            var allroles = await roleService.GetAllRoles();
            var role = allroles.FirstOrDefault(r => r.Name == rolevm.Name);
            if (role != null && role.Id != null && role.Name != null)
            {
                await roleService.DeleteRole(role.Id);
                ViewBag.Success = "Role deleted successfully!";
                var deleteRoleVM = new DeleteRole
                {
                    RolesList = await roleService.GetAllRoles()
                };
                return View("DeleteRole" , deleteRoleVM);
            }
            ViewBag.Error = "Invalid Id Or Name!";
            return View("DeleteRole", rolevm);

        }
        public async Task<IActionResult> Index()
        {
            var result = await roleService.GetAllRoles();
            return View(result);
        }
        
    }
}
