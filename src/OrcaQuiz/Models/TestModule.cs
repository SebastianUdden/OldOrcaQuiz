using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Models
{
    public class TestModule
    {
        public virtual List<Test> Tests { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }

        public TestModule()
        {
            Tests = new List<Test>();
        }
    }
}
