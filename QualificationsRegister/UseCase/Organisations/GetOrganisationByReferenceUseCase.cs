using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public class GetOrganisationByReferenceUseCase : IGetOrganisationByReferenceUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;

        public GetOrganisationByReferenceUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByReferenceUseCase>();
            _redisCacheService = redisCacheService;
        }

        public async Task<OrganisationPublic> GetOrganisationByReference(string reference)
        {
            var organisations = await _redisCacheService.GetCache<OrganisationPublic>("Organisations");

            var orgByRef = organisations.FirstOrDefault(e => e.RecognitionNumber == reference);

            return orgByRef;
        }

    }
}
