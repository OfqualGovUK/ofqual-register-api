using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Services;
using Ofqual.Common.RegisterAPI.Repository;

namespace Ofqual.Common.RegisterAPI.Functions
{
    public class CacheTimerTrigger
    {
        private readonly ILogger _logger;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IRegisterRepository _registerRepository;

        public CacheTimerTrigger(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService, IRegisterRepository repository)
        {
            _logger = loggerFactory.CreateLogger<CacheTimerTrigger>();
            _redisCacheService = redisCacheService;
            _registerRepository = repository;
        }

        [Function("CacheUpdateTimerTrigger")]
        public async Task RunAsync([TimerTrigger("0 0 5 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"Cache Update Timer trigger function executed at: {DateTime.Now}");
            Thread.Sleep(1000);
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            var organisations = await _registerRepository.GetOrganisations();
            var qualifications = await _registerRepository.GetQualifications();

            await _redisCacheService.RemoveAsync(RedisCache.Qualifications);
            await _redisCacheService.RemoveAsync(RedisCache.Organisations);

            await _redisCacheService.SetCacheAsync(RedisCache.Organisations, organisations.ToList());
            await _redisCacheService.SetCacheAsync(RedisCache.Organisations, qualifications.ToList());
        }
    }
}
