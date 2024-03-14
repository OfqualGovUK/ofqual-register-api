using Microsoft.EntityFrameworkCore;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Database
{
    public class RegisterDbContext : DbContext
    {
        public RegisterDbContext(DbContextOptions<RegisterDbContext> options) : base(options) { }

        public DbSet<DbQualification> Qualifications { get; set; }
        public DbSet<DbQualificationPublic> QualificationsPublic { get; set; }
        public DbSet<DbOrganisation> Organisations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbQualification>()
                .ToTable("Register_T_Qualification", "MD_Register")
                .HasKey(k => k.Id);

            modelBuilder.Entity<DbQualificationPublic>()
                .ToView("Public_Register_V_Qualification", "MD_Register")
                .HasKey(k => k.Id);

            modelBuilder.Entity<DbOrganisation>()
                .ToTable("Register_T_Organisation", "MD_Register")
                .HasKey(k => k.Id);
        }
    }
}
