using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations
{
    public class GetOrganisationsUseCase : IGetOrganisationsUseCase
    {
        private readonly ILogger _logger;
        private readonly RegisterDbContext _registerDbContext;

        public GetOrganisationsUseCase(ILoggerFactory loggerFactory, RegisterDbContext registerContext)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsUseCase>();
            _registerDbContext = registerContext;
        }

        public async Task<List<Organisation>> GetOrganisations(string search)
        {
            _logger.LogInformation("Getting list of orgs");
            return await _registerDbContext.Organisations.ToListAsync();
        }

    }
}
