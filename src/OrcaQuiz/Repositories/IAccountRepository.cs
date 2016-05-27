using Microsoft.AspNetCore.Identity;
using OrcaQuiz.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Repositories
{
    public interface IAccountRepository
    {
        void Register(RegistrationVM model);
        Task<SignInResult> SignIn(SignInVM model);

        void SignOut();
    }
}
