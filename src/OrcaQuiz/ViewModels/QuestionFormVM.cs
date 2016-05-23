using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TestPlatform.Models.Enums;

namespace TestPlatform.ViewModels
{
    public class QuestionFormVM
    {
        public string QuestionText { get; set; }
        public bool HasComment { get; set; }
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public string[] SelectedAnswers { get; set; }
        public QuestionType QuestionType { get; set; }
        public List<AnswerDetailVM> Answers { get; set; }
        public bool IsInTestSession { get; set; }
        public bool IsInEditQuestion { get; set; }
        public int? SortOrder { get; set; }
    }
}
