using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.ViewModels
{
    public class SignInVM
    {
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
