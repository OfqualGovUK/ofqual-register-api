using Microsoft.EntityFrameworkCore;


namespace Ofqual.Common.RegisterAPI.Services
{
    public class RegisterDbContext : DbContext
    {
        public RegisterDbContext(DbContextOptions<RegisterDbContext> options) : base(options) { }
    }
}
