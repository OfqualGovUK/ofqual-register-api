using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Ofqual.Common.RegisterAPI.Services.Cache;
//using Microsoft.Azure.Functions.Worker;

namespace Ofqual.Common.RegisterAPI.Functions
{
    public class CacheTrigger
    {
        private readonly ILogger _logger;
        private readonly IRedisCacheService _redisCacheService;

        public CacheTrigger(ILoggerFactory loggerFactory, IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
            _logger = loggerFactory.CreateLogger<CacheTrigger>();
        }


        [FunctionName("CacheRegisterRecords")]
        //[FixedDelayRetry(5, "00:00:10")]
        public async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {
        }
    }
}
