using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Services.Cache;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationsUseCase : IGetQualificationsUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;

        public GetQualificationsUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsUseCase>();
            _redisCacheService = redisCacheService;
        }

        public async Task<List<QualificationPublic>> GetQualificationsPublic(string? search)
        {
            var qualifications = await _redisCacheService.GetCacheAsync<Qualification>("Qualifications");
            _logger.LogInformation("Got Qualifications data. Converting to the public Model");

            var publicQualifications = qualifications.Select(e => new QualificationPublic(e)).ToList();
            _logger.LogInformation("Converted to the Qualifications Public Model");

            return publicQualifications;
        }

        public async Task<List<QualificationPrivate>> GetQualificationsPrivate(string? search)
        {
            var qualifications = await _redisCacheService.GetCacheAsync<Qualification>("Qualifications");

            _logger.LogInformation("Got Qualifications data. Converting to the Gov Model");
            var privateQualifications = qualifications.Select(e => new QualificationPrivate(e)).ToList();

            _logger.LogInformation("Converted to the Qualifications Gov Model");


            return privateQualifications;
        }

    }
}
