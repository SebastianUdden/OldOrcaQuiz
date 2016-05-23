using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;

namespace TestPlatform.ViewModels
{
    public class ModuleVM
    {
        public int Id { get; set; }
        [Display(Name = "Module name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public virtual List<Test> Tests { get; set; }
    }
}
