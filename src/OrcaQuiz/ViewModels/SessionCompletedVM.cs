using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.Models.Enums;

namespace OrcaQuiz.ViewModels
{
    public class SessionCompletedVM
    {
        public int TestSessionId { get; set; }
        public bool? IsSuccessful { get; set; }
        public bool HasCertificate { get; set; }
        public SessionCompletedReason SessionCompletedReason { get; set; }
        public string UserName { get; set; }
        public String Date { get; set; }
    }
}
