using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Database;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations
{
    public class GetOrganisationsUseCase : IGetOrganisationsUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;
        private readonly RegisterContext _registerContext;

        public GetOrganisationsUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService, RegisterContext registerContext)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsUseCase>();
            _redisCacheService = redisCacheService;
            _registerContext = registerContext;
        }

        public async Task<List<OrganisationPublic>> GetOrganisations(string search)
        {
            //var organisations = await _redisCacheService.GetCacheAsync<Organisation>("Organisations");
            _logger.LogInformation("Getting list of orgs");
            var orgs = await _registerContext.Organisations.ToListAsync();

            var publicOrganisations = orgs.Select(e => new OrganisationPublic(e)).ToList();

            return publicOrganisations;
        }

    }
}
