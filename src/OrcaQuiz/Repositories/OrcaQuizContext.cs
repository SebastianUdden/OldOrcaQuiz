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

        public OrcaQuizContext(DbContextOptions<OrcaQuizContext> options)
    : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Module>().ToTable("Modules");

        }
    }
}
