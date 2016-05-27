using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.ViewModels
{
    public class RegistrationVM
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "This field is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }
    }
}
