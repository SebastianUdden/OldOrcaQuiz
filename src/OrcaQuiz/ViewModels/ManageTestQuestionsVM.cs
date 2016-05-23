using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models;

namespace TestPlatform.ViewModels
{
    public class ManageTestQuestionsVM
    {
        public int TestId { get; set; }
        public string TestName { get; set; }
        public string Description { get; set; }
        public List<Question> Questions { get; set; }
        public QuestionFormVM QuestionFormVM { get; set; }
    }
}
