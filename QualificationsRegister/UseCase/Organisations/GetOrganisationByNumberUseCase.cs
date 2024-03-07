using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Repository;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;
        private readonly IRegisterRepository _registerRepository;

        public GetOrganisationByNumberUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService, IRegisterRepository repository)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByNumberUseCase>();
            _redisCacheService = redisCacheService;
            _registerRepository = repository;
        }

        public async Task<OrganisationPublic?> GetOrganisationByNumber(string number)
        {
            var organisations = await _redisCacheService.GetAndSetCacheAsync(RedisCache.Organisations, async () =>
            {
                return await _registerRepository.GetOrganisations();
            });

            var orgByRef = organisations.FirstOrDefault(e => e.RecognitionNumber.Equals(number, StringComparison.CurrentCultureIgnoreCase));
            return orgByRef == null ? null : new OrganisationPublic(orgByRef);
        }

    }
}
