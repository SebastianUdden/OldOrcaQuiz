using System;
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
        [Route("Admin/Register")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/Register")]
        public IActionResult Index(RegistrationVM model)
        { 
            accountRepository.Register(model);

            return View();
        }

        [Route("Admin/Signin")]
        public async Task<IActionResult> SignIn()
        {
            await accountRepository.SignIn(new SignInVM { UserName = "orca@quiz.com", Password = "P@ssw0rd" });
            return RedirectToAction(nameof(AdminController.Dashboard), "Admin");
        }

        [Route("Admin/Signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInVM model)
        {
            //if (!ModelState.IsValid)
            //    return View(model);
            
                model.UserName = "orca@quiz.com";
                model.Password = "P@ssw0rd";
            
            var result = await accountRepository.SignIn(model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("UserName", "User name or password was wrong");
                return View(model);
            }

            return RedirectToAction(nameof(AdminController.Dashboard), "Admin");
        }
    }
}
