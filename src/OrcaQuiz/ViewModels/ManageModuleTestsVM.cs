using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;

namespace TestPlatform.ViewModels
{
    public class ManageModuleTestsVM
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Description { get; set; }
        public List<Test> Tests { get; set; }
    }
}
