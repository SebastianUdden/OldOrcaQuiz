using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestPlatform.Utils;
using Microsoft.AspNetCore.Hosting;
using TestPlatform.Repositories;

namespace TestPlatform.Controllers
{
    public class PdfController : Controller
    {
        IHostingEnvironment env;
        ITestPlatformRepository repository;
        public PdfController(IHostingEnvironment env, ITestPlatformRepository repository)
        {
            this.repository = repository;
            this.env = env;
        }

        [Route("PDF/GetCertificate/{testSessionId}")]
        public IActionResult GetCertificate(int testSessionId)
        {
            var symbols = repository.GetCertificateSymbols(testSessionId);
            var bytes = PdfUtils.GenerateCerfificate(env, "OrcaQuizTemplate.pdf", symbols);
            
            return File(bytes, "application/pdf");
        }
    }
}
