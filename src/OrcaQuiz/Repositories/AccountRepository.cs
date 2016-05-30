using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OrcaQuiz.Models;
using Microsoft.AspNetCore.Identity;

namespace OrcaQuiz.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        IdentityDbContext identityContext;
        OrcaQuizContext orcaQuizContext;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;

        public AccountRepository(IdentityDbContext identityContext,
            OrcaQuizContext orcaQuizContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.identityContext = identityContext;
            this.orcaQuizContext = orcaQuizContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async void Register(RegistrationVM model)
        {
            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };
            var user = new User
            {
                FirstName = model.FirstName,
                Lastname = model.LastName,
                UserId = identityUser.Id
            };

            var result = await userManager.CreateAsync(identityUser, model.Password);


            if (result.Succeeded)
            {
                orcaQuizContext.Users.Add(user);
                await orcaQuizContext.SaveChangesAsync();

                await signInManager.SignInAsync(identityUser, isPersistent: false);
            }
        }

        public async Task<SignInResult> SignIn(SignInVM model)
        {
            return await signInManager.PasswordSignInAsync(model.Username, model.Password, isPersistent: false, lockoutOnFailure: false);
        }

        public async void SignOut()
        {
            await signInManager.SignOutAsync();
        }
    }
}
