using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.Services.Repository;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations
{
    public class GetOrganisationsUseCase : IGetOrganisationsUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IRegisterRepository _registerRepository;
        private readonly ILogger _logger;

        public GetOrganisationsUseCase(ILoggerFactory loggerFactory, IRegisterRepository registerRepository,
            IRedisCacheService redisCacheService)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsUseCase>();
            _registerRepository = registerRepository;   
            _redisCacheService = redisCacheService;
        }

        public async Task<IEnumerable<Organisation>> GetOrganisations(string search)
        {
            var organisations = await _redisCacheService.GetCacheAsync<Organisation>("Organisations");
            if (organisations == null)
            {
                //cache missed
                _logger.LogInformation("Cache Miss");
                organisations = await _registerRepository.GetOrganisations();
                await _redisCacheService.SetCacheAsync("organisations", organisations);

            }
            //filters
            var privateOrganisations = organisations.Select(e => new OrganisationPrivate(e));
            return privateOrganisations;
        }
    }
}
