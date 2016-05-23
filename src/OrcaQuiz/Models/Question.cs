using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrcaQuiz.Models.Enums;

namespace OrcaQuiz.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int? TestId { get; set; }
        public int? SortOrder { get; set; }
        public string Name { get; set; }
        public string QuestionText { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Author { get; set; }
        public string Tags { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool HasComment { get; set; }
        public virtual List<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }
    }
}
