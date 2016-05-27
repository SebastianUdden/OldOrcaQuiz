using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        //public virtual IdentityUser IdentityUser { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public virtual List<TestSession> TestSessions { get; set; }

        public User()
        {
            TestSessions = new List<TestSession>();
        }
    }
}
