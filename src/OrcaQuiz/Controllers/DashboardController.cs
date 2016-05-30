using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OrcaQuiz.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Controllers
{
    [Authorize]
    public class DashboardController: Controller
    {
        IOrcaQuizRepository repository;
        IHostingEnvironment env;

        public DashboardController(IOrcaQuizRepository repository, IHostingEnvironment env)
        {
            this.env = env;
            this.repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var model = await repository.GetDashboardVM(User.Identity.Name);
            return View(model);
        }
    }
}
