﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OrcaQuiz.ViewModels;
using OrcaQuiz.Repositories;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OrcaQuiz.Controllers
{
    public class AccountController : Controller
    {

        IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        // GET: /<controller>/
        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/Register")]
        public IActionResult Register(RegistrationVM model)
        { 
            accountRepository.Register(model);

            return View();
        }

        [Route("Account/Signin")]
        public async Task<IActionResult> SignIn()
        {
            var autoSignIn = true;
            if (autoSignIn)
            {
                await accountRepository.SignIn(new SignInVM { Username = "orca@quiz.com", Password = "P@ssw0rd" });
                return RedirectToAction(nameof(AdminController.Dashboard), "Admin");
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
                ModelState.AddModelError("UserName", "User name or password was wrong");
                return View(model);
            }

            return RedirectToAction(nameof(AdminController.Dashboard), "Admin");
        }
        [Route("Account/Signout")]
        public IActionResult SignOut()
        {
            accountRepository.SignOut();
            return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}
