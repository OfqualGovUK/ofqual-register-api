using Microsoft.EntityFrameworkCore;
using Ofqual.Common.RegisterAPI.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Database
{
    public class RegisterContext : DbContext
    {
        public RegisterContext(DbContextOptions<RegisterContext> options) : base(options) { }

        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Organisation> Organisations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Qualification>()
                .ToView("Register_V_Qualification", "MD_Register")
                .HasKey(k => k.Id);

            modelBuilder.Entity<Organisation>()
                .ToView("Register_V_Organisation", "MD_Register")
                .HasKey(k => k.Id);
        }
    }
}
