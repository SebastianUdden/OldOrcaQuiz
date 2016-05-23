using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestPlatform.ViewModels
{
    public class ViewQuestionVM
    {
        public int TestId { get; set; }
        public string TestTitle { get; set; }
        public int QuestionIndex { get; set; }
        public int NumOfQuestion { get; set; }
        public double? SecondsLeft { get; set; }
        public QuestionFormVM QuestionFormVM { get; set; }
    }
}
