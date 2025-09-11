using Cars.BLL.ModelVM.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars.BLL.Service.Abstraction
{
    public interface IAccountService
    {
        Task<IdentityResult> SignUp(SignUpVM signUp);
        Task<IdentityResult> SignUpMechanic(SignUpMechanicVM signUp);
        Task<SignInResult> SignIn(SignInVM signIn);
        //Task<SignInResult> SignInMechanic(SignInVM signIn);
        Task LogOut();

        //Task LogOutMechanic();
        Task<(bool , AppUser)> ForgetPassword(ForgetPasswordVM forgetPassword);
    }
}
