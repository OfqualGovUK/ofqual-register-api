using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Services;

namespace Ofqual.Common.RegisterAPI.Functions
{
    public class CacheTimerTrigger
    {
        private readonly ILogger _logger;
        private readonly IRedisCacheService _redisCacheService;

        public CacheTimerTrigger(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService)
        {
            _logger = loggerFactory.CreateLogger<CacheTimerTrigger>();
            _redisCacheService = redisCacheService;
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

            await _redisCacheService.ResetCacheAsync();
        }
    }
}
