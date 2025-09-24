namespace Cars.BLL.Service.Abstraction
{
    public interface IAccountService
    {
        Task<IdentityResult> SignUp(SignUpVM signUp);
        //Task<IdentityResult> SignUpMechanic(SignUpMechanicVM signUp);
        Task<Microsoft.AspNetCore.Identity.SignInResult> SignIn(SignInVM signIn);
        //Task<SignInResult> SignInMechanic(SignInVM signIn);
        Task LogOut();

        //Task LogOutMechanic();
        Task<(bool , AppUser)> ForgetPassword(ForgetPasswordVM forgetPassword);
    }
}
