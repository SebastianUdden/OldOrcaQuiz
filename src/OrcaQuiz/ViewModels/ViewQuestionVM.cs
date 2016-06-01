using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.ViewModels
{
    public class ViewQuestionVM
    {
        public int TestId { get; set; }
        public string TestTitle { get; set; }
        public int QuestionIndex { get; set; }
        public int NumOfQuestion { get; set; }
        public int? SecondsLeft { get; set; }
        public DateTime TimeOfTestStart { get; set; }
        public bool HasTimer { get; set; }
        //public double? SecondsLeft { get; set; }
        public QuestionFormVM QuestionFormVM { get; set; }
    }
}
