using Microsoft.EntityFrameworkCore;
using Ofqual.Common.RegisterAPI.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Database
{
    public class RegisterDbContext : DbContext
    {
        public RegisterDbContext(DbContextOptions<RegisterDbContext> options) : base(options) { }

        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<QualificationPublic> QualificationsPublic { get; set; }
        public DbSet<Organisation> Organisations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Qualification>()
                .ToTable("Register_T_Qualification", "MD_Register")
                .HasKey(k => k.Id);
            
            modelBuilder.Entity<QualificationPublic>()
                .ToView("Public_Register_V_Qualification", "MD_Register")
                .HasKey(k => k.Id);

            modelBuilder.Entity<Organisation>()
                .ToTable("Register_T_Organisation", "MD_Register")
                .HasKey(k => k.Id);
        }
    }
}
