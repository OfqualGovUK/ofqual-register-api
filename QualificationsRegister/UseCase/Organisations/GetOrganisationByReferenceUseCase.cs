using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models;
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

        public async Task<Organisation> GetOrganisationByReference(string reference)
        {
            var organisations = await _redisCacheService.GetCache<Organisation>("Organisations");

            var orgByNumber = organisations.FirstOrDefault(e => e.RecognitionNumber == reference);

            return orgByNumber;
        }

    }
}
