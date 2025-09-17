using AutoMapper;
using Cars.BLL.ModelVM.Account;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Cars.PL.Language;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Cars.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        private readonly IStringLocalizer<SharedResource> SharedLocalizer;

        private readonly IRoleService RoleService;

        private readonly UserManager<AppUser> UserManager;
        private readonly IEmailService EmailService;
        

        public AccountController(IAccountService accountService, IMapper mapper , IStringLocalizer<SharedResource> SharedLocalizer , IRoleService roleService , UserManager<AppUser> userManager, IEmailService EmailService)
        {
            this.accountService = accountService;
            this.mapper = mapper;
            this.SharedLocalizer = SharedLocalizer;
            RoleService = roleService;
            UserManager = userManager;
            this.EmailService = EmailService;
            
        }
        public IActionResult determineRole()
        {
            return View();
        }
        public IActionResult SignUpView(string role)
        {
            SignUpVM signUp = new SignUpVM();
            signUp.Role = role;
            return View(signUp);
        }
        //[HttpGet]
        //public IActionResult SignUpMechanic()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> SignUpMechanic(SignUpMechanicVM signUp)
        //{
        //    var result = await accountService.SignUpMechanic(signUp);
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByEmailAsync(signUp.Email);
        //        if (user != null && user.Email != null)
        //        {
        //            var role = await RoleService.AssignRoleToUser(user, "Mechanic");
        //        }
        //        return RedirectToAction("Login");
        //    }
        //    else
        //    {
        //        foreach (var item in result.Errors)
        //        {
        //            ModelState.AddModelError("Password", item.Description);
        //        }
        //    }
        //    ViewBag.error = "Some Thing Is Wrong!";
        //    return View("SignUpView", signUp);
        //}
        public async Task<IActionResult> SignUp(SignUpVM signUp)
        {
            var result = await accountService.SignUp(signUp);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByEmailAsync(signUp.Email);
                if (user != null && user.Email != null)
                {
                    var role = await RoleService.AssignRoleToUser(user, signUp.Role);
                }
                ViewBag.success = "SignUp Is Done Successfully , You Can Verify Your Account Now";
                return RedirectToAction("SendEmailConfirm", "Account" , new {email = signUp.Email});
                //return View("LoginView");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }
            ViewBag.error = "Some Thing Is Wrong!";
            return View("SignUpView" , signUp);
        }
        public IActionResult LoginView()
        {
            return View();
        }
        public async Task<IActionResult> Login(SignInVM signin)
        {
            var result = await accountService.SignIn(signin);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                
                ModelState.AddModelError("", "Invalid UserName Or Password");
                ViewBag.error = SharedLocalizer["Some Thing Is Wrong!"];
                return View("LoginView", signin);
            }
            
        }
        public IActionResult LogOutView()
        {
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await accountService.LogOut();
            return RedirectToAction("Index", "Home");
        }
        public async Task<ActionResult> SendEmailConfirm(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (ModelState.IsValid && user != null)
            {
                var resetToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                //**add new object after "EditUserProfile"**
                var resetLink = Url.Action("ConfirmEmail", "Account", new { email = email, token = resetToken }, protocol: HttpContext.Request.Scheme);

                var subject = "Vrify Your Email";
                var body = $"Please, Vrify Your Email By Clicking Here : <a href=\"{resetLink}\">Vrify Email</a>";

                await EmailService.SendEmail(email, subject, body);
                ViewBag.Send = "Verification Email Sent";
                return View();
            }
            else
            {
                ViewBag.Error = "Email Not Found!";
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (email == null || token == null)
            {
                ViewBag.Error = "Email Not Found!";
                return View("SendEmailConfirm");
            }

            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Error = "Email Not Found!";
                return View("SendEmailConfirm");
            }

            var result = await UserManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.Success = "Your Email Verified Successfully";
                return View("LoginView"); 
            }

            ViewBag.Error = "Some Thing Was Wrong";
            return View("SendEmailConfirm");
        }




    }
}
