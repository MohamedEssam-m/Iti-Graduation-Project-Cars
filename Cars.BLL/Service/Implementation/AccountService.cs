namespace Cars.BLL.Service.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMapper mapper;

        public AccountService(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }



        public async Task LogOut()
        {
            try
            {
                await signInManager.SignOutAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> SignIn(SignInVM signIn)
        {
            try
            {
                var result = await signInManager.PasswordSignInAsync(signIn.UserName, signIn.Password, false , false);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Microsoft.AspNetCore.Identity.SignInResult();
            }
        }
        //public async Task<SignInResult> SignInMechanic(SignInVM signIn)
        //{
        //    try
        //    {
        //        var result = await signInManager.PasswordSignInAsync(signIn.Email, signIn.Password, false, false);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new SignInResult();
        //    }
        //}


        public async Task<IdentityResult> SignUp(SignUpVM signUp)
        {
            try
            {
                if(signUp.Role == "User" || signUp.Role == "Admin")
                {
                    var user = mapper.Map<AppUser>(signUp);

                    var result = await userManager.CreateAsync(user, signUp.Password);
                    return result;
                }
                else
                {
                    var user = mapper.Map<MechanicUser>(signUp);
                    user.UserName = signUp.UserName;
                    user.Address = signUp.Address;
                    user.FullName = signUp.Fullname;

                    var result = await userManager.CreateAsync(user, signUp.Password);
                    return result;
                }
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new IdentityResult();
            }
        }

        //public async Task<IdentityResult> SignUpMechanic(SignUpMechanicVM signUp)
        //{
        //    try
        //    {
        //        var user = new MechanicUser()
        //        {
        //            UserName = signUp.Email,
        //            Email = signUp.Email
        //        };

        //        var result = await userManager.CreateAsync(user, signUp.Password);
        //        return result;

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return new IdentityResult();
        //    }
        //}
        public async Task<(bool , AppUser)> ForgetPassword(ForgetPasswordVM forgetPassword)
        {
            var user = await userManager.FindByEmailAsync(forgetPassword.Email);
            if (user == null || user.Email == null)
                return (false , new AppUser());
            return (true , user);
        }
    }
}
