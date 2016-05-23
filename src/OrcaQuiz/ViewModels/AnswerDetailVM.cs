using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.Models.Enums;

namespace OrcaQuiz.ViewModels
{
    public class AnswerDetailVM
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool ShowAsCorrect { get; set; }
        public bool IsChecked { get; set; }
        public bool IsInPreview { get; set; }
        public int SortOrder { get; set; }
        public QuestionType QuestionType { get; set; }
    }
}
