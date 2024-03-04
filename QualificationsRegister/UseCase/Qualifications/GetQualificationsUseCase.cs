using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.Repository;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationsUseCase : IGetQualificationsUseCase
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger _logger;
        private readonly IRegisterRepository _registerRepository;

        public GetQualificationsUseCase(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService, IRegisterRepository repository)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationsUseCase>();
            _redisCacheService = redisCacheService;
            _registerRepository = repository;
        }

        public async Task<List<QualificationPublic>> GetQualificationsPublic(string? search)
        {
            var qualifications = await _redisCacheService.GetAndSetCacheAsync(RedisCache.Qualifications, async () =>
            {
                return await _registerRepository.GetQualifications();
            });

            _logger.LogInformation("Got Qualifications data. Converting to the public Model");
            return qualifications.Select(e => new QualificationPublic(e)).ToList()!;
        }

        public async Task<List<QualificationPrivate>> GetQualificationsPrivate(string? search)
        {
            var qualifications = await _redisCacheService.GetAndSetCacheAsync(RedisCache.Qualifications, async () =>
            {
                return await _registerRepository.GetQualifications();
            });

            _logger.LogInformation("Got Qualifications data. Converting to the Gov Model");
            return qualifications.Select(e => new QualificationPrivate(e)).ToList();
        }

    }
}
