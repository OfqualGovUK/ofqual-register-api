using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;

        public GetOrganisationByNumberUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByNumberUseCase>();
            _redisCacheService = redisCacheService;
        }

        public async Task<OrganisationPublic?> GetOrganisationByNumber(string number)
        {
            var organisations = await _redisCacheService.GetCacheAsync<Organisation>("Organisations");

            var orgByRef = organisations.FirstOrDefault(e => e.RecognitionNumber.Equals(number, StringComparison.CurrentCultureIgnoreCase));

            return orgByRef == null ? null : new OrganisationPublic(orgByRef);
        }

    }
}
