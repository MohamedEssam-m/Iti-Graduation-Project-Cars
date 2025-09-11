using AutoMapper;
using Cars.BLL.ModelVM.Account;
using Cars.BLL.ModelVM.AppUserVM;
using Cars.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class EditUserProfileController : Controller
    {
        private readonly IAppUserService UserService;
        private readonly IMapper Mapper;
        private readonly IAccountService accountService;
        private readonly UserManager<AppUser> userManager;

        public EditUserProfileController(IAppUserService userService , IMapper mapper , IAccountService accountService,UserManager<AppUser> userManager)
        {
            UserService = userService;
            Mapper = mapper;
            this.accountService = accountService;
            this.userManager = userManager;
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public async Task<IActionResult> SaveChangePassword(ForgetPasswordVM forgetPassword)
        {
            if (ModelState.IsValid)
            {
                if (forgetPassword != null && forgetPassword.Email != null && forgetPassword.Password == forgetPassword.ConfirmPassword)
                {
                    (bool Return, var user) = await accountService.ForgetPassword(forgetPassword);
                    if (user != null)
                    {
                        var result = await userManager.ResetPasswordAsync(user, forgetPassword.Token, forgetPassword.Password);

                        if (result.Succeeded)
                        {
                            ViewBag.success = "Password Changed Successfully";
                            return View("ChangePassword", forgetPassword);
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("ChangePassword", forgetPassword);
        }
        public IActionResult EditUserProfileView()
        {
            return View();
        }
        public async Task<IActionResult> SaveEditProfile(UpdateUserVM user)
        {
            if(ModelState.IsValid)
            {
                if(user != null)
                {
                    var result = await UserService.UpdateUser(user);
                    if(result)
                    {
                        ViewBag.success = "Profile Updated Successfully";
                        return View("EditUserProfileView");
                    }
                }
                
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("EditUserProfileView", user);

        }

    }
}
