using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.ViewModels
{
    public class TestSettingsFormVM
    {
        public int? Id { get; set; }

        [Display(Name = "Test Name")]
        [Required(ErrorMessage = "The test must have a name")]
        public string TestName { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "The test must have a description")]
        public string Description { get; set; }

        [Display(Name = "Time limit in minutes)")]
        [Range(1, 1440, ErrorMessage = "Number must be between 1 and 1440 (24h)")]
        public int? TimeLimitInMinutes { get; set; }

        [Display(Name = "Tags")]
        public string Tags { get; set; }

        [Display(Name = "Show pass or fail")]
        public bool ShowPassOrFail { get; set; }

        [Display(Name = "Show test score")]
        public bool ShowTestScore { get; set; }

        [Display(Name = "Pass threshhold (%)")]
        public int? PassPercentage { get; set; }

        [Display(Name = "Custom message")]
        public string CustomCompletionMessage { get; set; }

        [Display(Name = "Name of certificate PDF template (*.pdf)")]
        public string CertificateTemplatePath { get; set; }

        [Display(Name = "Enable certificate download")]
        public bool EnableCertificateDownloadOnCompletion { get; set; }

        [Display(Name = "Enable certificate by email")]
        public bool EnableEmailCertificateOnCompletion { get; set; }

        [Display(Name = "Company Name")]
        public string CertificateCompany { get; set; }

        [Display(Name = "Author")]
        public string CertificateAuthor { get; set; }

        [Display(Name = "Custom Text")]
        public string CertificateCustomText { get; set; }

        [Display(Name = "Number of active questions per test")]
        [Range(1, 1000, ErrorMessage = "Number must be between 1 and 1000")]
        public int? NumberOfFeaturedQuestions { get; set; }

        public string UserName { get; set; }
    }
}
