using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Database
{
    public class RegisterDb : IRegisterDb
    {
        private RegisterDbContext _context;
        private ILogger _logger;

        public RegisterDb(RegisterDbContext registerDbContext, ILoggerFactory loggerFactory) {
            _context = registerDbContext;
            _logger = loggerFactory.CreateLogger<RegisterDb>();
        }

        public async Task<List<Organisation>> GetOrganisations()
        {
            return  await _context.Organisations.ToListAsync();
        }

        public async Task<List<Qualification>> GetQualifications()
        {
            return await _context.Qualifications.ToListAsync();
        }

        public async Task<List<QualificationPublic>> GetQualificationsPublic()
        {
            return await _context.QualificationsPublic.ToListAsync();
        }
    }
}
