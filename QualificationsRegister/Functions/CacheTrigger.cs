using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.Functions.Worker;

namespace Ofqual.Common.RegisterAPI.Functions
{
    public class CacheTrigger
    {
        private readonly ILogger _logger;
        //private readonly  rediscache
        public CacheTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CacheTrigger>();
        }


        [FunctionName("CacheRegisterRecords")]
        //[FixedDelayRetry(5, "00:00:10")]
        public async Task RunAsync([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
        {

        }
    }
}
