using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrcaQuiz.Utils;
using Microsoft.AspNetCore.Hosting;
using OrcaQuiz.Repositories;

namespace OrcaQuiz.Controllers
{
    public class PdfController : Controller
    {
        IHostingEnvironment env;
        IOrcaQuizRepository repository;
        public PdfController(IHostingEnvironment env, IOrcaQuizRepository repository)
        {
            this.repository = repository;
            this.env = env;
        }

        [Route("PDF/GetCertificate/{testSessionId}")]
        public IActionResult GetCertificate(int testSessionId)
        {
            var symbols = repository.GetCertificateSymbols(testSessionId);
            var bytes = PdfUtils.GenerateCerfificate(env, symbols.TemplateName, symbols);
            
            return File(bytes, "application/pdf");
        }
    }
}