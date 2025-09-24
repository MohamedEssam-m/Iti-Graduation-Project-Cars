namespace Cars.PL.Controllers
{
    public class EditUserProfileController : Controller
    {
        private readonly IAppUserService UserService;
        private readonly IMapper mapper;
        private readonly IAccountService accountService;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService EmailService;
        private readonly IConfiguration configuration;


        public EditUserProfileController(IConfiguration configuration,IAppUserService userService , IMapper mapper , IAccountService accountService,UserManager<AppUser> userManager, IEmailService EmailService)
        {
            UserService = userService;
            this.mapper = mapper;
            this.accountService = accountService;
            this.userManager = userManager;
            this.EmailService = EmailService;
            this.configuration = configuration; 
        }
        public IActionResult ChangePassword(string email , string token)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty (email) && !string.IsNullOrEmpty(token))
            {
                var ModelToChangePassword = new ForgetPasswordVM { Email = email, Token = token };
               
                return View(ModelToChangePassword);
            }
            else
            {
                var model = new VerifyEmail { Email = email, Token = token };
                ViewBag.Error = "Invalid Email!";
                return View("VerifyEmail", model); 
            }
        }
        public async Task<IActionResult> SaveChangePassword(ForgetPasswordVM forgetPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(forgetPassword.Email);
                if (user == null)
                {
                    var model = mapper.Map<VerifyEmail>(forgetPassword);
                    ViewBag.Error = "Invalid Email!";
                    return View("VerifyEmail", model);

                }
                var result = await userManager.ResetPasswordAsync(user , forgetPassword.Token , forgetPassword.ConfirmPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ViewBag.success = "Password Changed Successfully";
                    return View("LoginView");
                }
                
                //if (forgetPassword != null && forgetPassword.Email != null && forgetPassword.Password == forgetPassword.ConfirmPassword)
                //{
                //    (bool Return, var user) = await accountService.ForgetPassword(forgetPassword);
                //    if (user != null)
                //    {
                //        var result = await userManager.ResetPasswordAsync(user, forgetPassword.Token, forgetPassword.Password);

                //        if (result.Succeeded)
                //        {
                //            ViewBag.success = "Password Changed Successfully";
                //            return View("ChangePassword", forgetPassword);
                //        }

                //        foreach (var error in result.Errors)
                //        {
                //            ModelState.AddModelError("", error.Description);
                //        }
                //    }
                //}
            }
            ViewBag.Error = "Some Thing Was Wrong";
            return View("ChangePassword", forgetPassword);
        }
        public async Task<IActionResult> EditUserProfileView()
        {
            string UserId =  userManager.GetUserId(User);
            var user = await UserService.GetById(UserId);
            var updateUserVM = mapper.Map<UpdateUserVM>(user);
            //updateUserVM.User_Image_Path = user.UserImagePath;
            return View(updateUserVM);
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
        public ActionResult VerifyEmail()
        {
            return View();
        }
        public async Task<ActionResult> EmailSend(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (ModelState.IsValid && user != null)
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                //**add new object after "EditUserProfile"**
                var resetLink = Url.Action("ChangePassword", "EditUserProfile",new { email = email, token = resetToken }, protocol: HttpContext.Request.Scheme);

                var subject = "Reset Password";
                var body = $"Please, Reset Your Password By Clicking Here : <a href=\"{resetLink}\">Reset Password</a>";

                await EmailService.SendEmail(email , subject , body , configuration["EmailSettings:From"]);
                return View();
            }
            else
            {
                ViewBag.Error = "Email Not Found!";
                return View("VerifyEmail");
            }
        }

    }
}
