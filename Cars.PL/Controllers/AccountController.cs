using AutoMapper;
using Cars.BLL.ModelVM.Account;
using Cars.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cars.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
            this.mapper = mapper;
        }
        public IActionResult determineRole()
        {
            return View();
        }
        public IActionResult SignUpView()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SignUpMechanic()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUpMechanicVM(SignUpMechanicVM signUp)
        {
            var result = await accountService.SignUpMechanic(signUp);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }
            ViewBag.error = "Some Thing Is Wrong!";
            return View("SignUpView", signUp);
        }
        public async Task<IActionResult> SignUp(SignUpVM signUp)
        {
            var result = await accountService.SignUp(signUp);
            if (result.Succeeded)
            {
                return RedirectToAction("LoginView");
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
                return View("LoginView", signin);
            }
            
        }
        public async Task<IActionResult> LogOut()
        {
            await accountService.LogOut();
            return RedirectToAction("Index", "Home");
        }




    }
}
