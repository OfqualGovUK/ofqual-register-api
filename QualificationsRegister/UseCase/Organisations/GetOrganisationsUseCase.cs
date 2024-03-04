using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Repository;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations
{
    public class GetOrganisationsUseCase : IGetOrganisationsUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;
        private readonly IRegisterRepository _registerRepository;

        public GetOrganisationsUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService, IRegisterRepository repository)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationsUseCase>();
            _redisCacheService = redisCacheService;
            _registerRepository = repository;
        }

        public async Task<List<OrganisationPublic>> GetOrganisations(string search)
        {
            var organisations = await _redisCacheService.GetAndSetCacheAsync(RedisCache.Organisations, async () =>
            {
                return await _registerRepository.GetOrganisations();
            });

            return organisations.Select(e => new OrganisationPublic(e)).ToList();
        }

    }
}
