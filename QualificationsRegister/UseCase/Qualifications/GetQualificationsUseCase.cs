using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.Private;
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

        public async Task<IEnumerable<QualificationPrivate>> GetQualifications(string search)
        {
            return  await _redisCacheService.GetCacheAsync<QualificationPrivate>("Qualifications");
        }
    }
}
