using Microsoft.EntityFrameworkCore;
using OrcaQuiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrcaQuiz.Repositories
{
    public class OrcaQuizContext : DbContext
    {
        public DbSet<Module> Modules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public OrcaQuizContext(DbContextOptions<OrcaQuizContext> options)
    : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Module>().ToTable("Modules");
            modelBuilder.Entity<User>().ToTable("Users").HasKey("Id");
            modelBuilder.Entity<Test>().ToTable("Tests");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Answer>().ToTable("Answers");


        }
    }
}
