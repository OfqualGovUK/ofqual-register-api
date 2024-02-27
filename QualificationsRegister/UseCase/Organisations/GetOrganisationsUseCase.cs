using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations
{
    public class GetOrganisationsUseCase : IGetOrganisationsUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;

        public GetOrganisationsUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsUseCase>();
            _redisCacheService = redisCacheService;
        }

        public async Task<IEnumerable<OrganisationPublic>> GetOrganisations(string search)
        {
            var organisations = await _redisCacheService.GetCache<Organisation>("Organisations");

            var publicOrganisations = organisations.Select(e => new OrganisationPublic(e));

            return publicOrganisations;
        }

    }
}
