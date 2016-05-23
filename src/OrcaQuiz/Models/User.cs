using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public List<TestSession> TestSessions { get; set; }

        public User()
        {
            TestSessions = new List<TestSession>();
        }
    }
}
