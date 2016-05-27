using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Models
{
    public class Test
    {
        public int Id { get; set; }
        public int? ModuleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; } //Sekundärnyckel till användaren
        public string Tags { get; set; }
        public int SordOrder { get; set; }
        public bool IsPublished { get; set; }
        public virtual List<Question> Questions { get; set; }
        public int? TimeLimitInMinutes { get; set; }
        public bool ShowPassOrFail { get; set; }
        public bool ShowTestScore { get; set; }
        public int? PassPercentage { get; set; }
        public string CustomCompletionMessage { get; set; }
        public string CertificateTemplatePath { get; set; }
        public bool EnableCertificateDownloadOnCompletion { get; set; }
        public bool EnableEmailCertificateOnCompletion { get; set; }
        public virtual ICollection<TestSession> TestSessions { get; set; }
        public string CertificateCompany { get; set; }
        public string CertificateAuthor { get; set; }
        public string CertificateCustomText { get; set; }
        public int? NumberOfFeaturedQuestions { get; set; }

        public Test()
        {
            Questions = new List<Question>();
        }
    }
}
