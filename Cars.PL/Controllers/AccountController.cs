using AutoMapper;
using Cars.BLL.ModelVM.Account;
using Cars.BLL.ModelVM.Offers;
using Cars.BLL.Service.Abstraction;
using Cars.BLL.Service.Implementation;
using Cars.PL.Language;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

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
        private readonly SignInManager<AppUser> signInManager;
        private readonly IConfiguration configuration;



        public AccountController(IConfiguration configuration,SignInManager<AppUser> signInManager,IAccountService accountService, IMapper mapper , IStringLocalizer<SharedResource> SharedLocalizer , IRoleService roleService , UserManager<AppUser> userManager, IEmailService EmailService)
        {
            this.accountService = accountService;
            this.mapper = mapper;
            this.SharedLocalizer = SharedLocalizer;
            RoleService = roleService;
            UserManager = userManager;
            this.EmailService = EmailService;
            this.signInManager = signInManager;
            this.configuration = configuration;
            
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
        public async Task<ActionResult> SendEmailConfirm(string Toemail)
        {
            var user = await UserManager.FindByEmailAsync(Toemail);
            if (ModelState.IsValid && user != null)
            {
                var resetToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                //**add new object after "EditUserProfile"**
                var resetLink = Url.Action("ConfirmEmail", "Account", new { email = Toemail, token = resetToken }, protocol: HttpContext.Request.Scheme);

                var subject = "Vrify Your Email";
                var body = $"Please, Vrify Your Email By Clicking Here : <a href=\"{resetLink}\">Vrify Email</a>";

                await EmailService.SendEmail(Toemail, subject, body , configuration["EmailSettings:From"]);
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
       
        [HttpGet]
        public IActionResult GoogleSignIn()
        {
            
            var redirectUrl = Url.Action("GoogleResponse", "Account");
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

            
            return Challenge(properties, "Google");
        }


        
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
        
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
        
                ViewBag.Error = "Google login info not found (external cookie missing or SignInScheme not configured).";
                return RedirectToAction("Login", "Account");
            }

        
            var provider = info.LoginProvider; 
            var providerKey = info.ProviderKey; 
            var claimsList = info.Principal.Claims.Select(c => new { c.Type, c.Value }).ToList();
            

            
            var signInResult = await signInManager.ExternalLoginSignInAsync(provider, providerKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                
                return RedirectToAction("Index", "Home");
            }

            
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (!string.IsNullOrEmpty(email))
            {
                var existingUserByEmail = await UserManager.FindByEmailAsync(email);
                if (existingUserByEmail != null)
                {
            
                    var addLoginResult = await UserManager.AddLoginAsync(existingUserByEmail, info);
                    if (addLoginResult.Succeeded)
                    {
            
                        await signInManager.SignInAsync(existingUserByEmail, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
            
                        var errors = string.Join("; ", addLoginResult.Errors.Select(e => e.Description));
                        ViewBag.Error = "Failed to link external login to existing account: " + errors;
                        return RedirectToAction("Login", "Account");
                    }
                }
            }

            
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? email;
            var newUser = new AppUser
            {
                UserName = email ?? Guid.NewGuid().ToString(),
                Email = email,
                FullName = name,
                EmailConfirmed = true 
            };

            var createResult = await UserManager.CreateAsync(newUser);
            if (!createResult.Succeeded)
            {
                
                var createErrors = string.Join("; ", createResult.Errors.Select(e => e.Description));
                ViewBag.Error = "User creation failed: " + createErrors;
                
                return RedirectToAction("Login", "Account");
            }

            
            var addLogin = await UserManager.AddLoginAsync(newUser, info);
            if (!addLogin.Succeeded)
            {
                var addLoginErrors = string.Join("; ", addLogin.Errors.Select(e => e.Description));
                ViewBag.Error = "Failed to add external login after creating user: " + addLoginErrors;
                return RedirectToAction("Login", "Account");
            }

            
            await signInManager.SignInAsync(newUser, isPersistent: false);

            
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return RedirectToAction("Index", "Home");
        }



        [HttpGet("Logout")]
        public async Task<IActionResult> GoogleLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        public async Task<ActionResult> ContactEmail(string userEmail , string subject , string message)
        {
            var user = await UserManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                ViewBag.Error = "User not found!";
                return View("CreateView", new CreateOfferVM());
            }

            await EmailService.SendEmail("zeyadazzap0@gmail.com", subject, message , userEmail);

            ViewBag.Success = "Email Sent Successfully";
            return View("Contact");
        }




    }
}
