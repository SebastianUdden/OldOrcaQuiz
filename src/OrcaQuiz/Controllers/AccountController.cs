using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OrcaQuiz.ViewModels;
using OrcaQuiz.Repositories;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OrcaQuiz.Controllers
{
    public class AccountController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<IdentityUser> userManager;
        IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.accountRepository = accountRepository;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        // GET: /<controller>/
        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/Register")]
        public async Task<IActionResult> Register(RegistrationVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await accountRepository.Register(model);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
            else
            {
                ModelState.AddModelError(String.Empty, result.Errors.First().Description);
                return View(model);
            }
        }

        [Route("Account/Signin")]
        public async Task<IActionResult> SignIn()
        {
            // Ändra för development/user
            var autoSignIn = false;

            if (autoSignIn)
            {
                await accountRepository.SignIn(new SignInVM { Username = "orca@quiz.com", Password = "P@ssw0rd" });
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }

            return View();
        }

        [Route("Account/Signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await accountRepository.SignIn(model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "User name or password was wrong");
                return View(model);
            }

            //var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
            //if (roleResult.Succeeded)
            //{
            //    var user = await userManager.FindByNameAsync(model.Username);
            //    var userRoleResult = await userManager.AddToRoleAsync(user, "Admin");
            //}

            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        [Route("Account/Signout")]
        public IActionResult SignOut()
        {
            accountRepository.SignOut();
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}
