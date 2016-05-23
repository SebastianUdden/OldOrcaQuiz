using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OrcaQuiz.Controllers
{
    public class AccountController : Controller
    {

        IdentityDbContext idContext;

        public AccountController(IdentityDbContext idContext)
        {
            this.idContext = idContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
