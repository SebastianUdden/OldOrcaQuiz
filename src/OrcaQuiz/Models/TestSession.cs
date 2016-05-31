using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Models
{
    public class TestSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public virtual List<QuestionResult> QuestionResults { get; set; }
        //public double? SecondsLeft { get; set; }

        //public TestSession()
        //{
        //    QuestionResults = new List<QuestionResult>();
        //}

    }
}
