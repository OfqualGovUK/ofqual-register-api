using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Ofqual.Common.RegisterAPI.Functions
{
    public class HealthCheck
    {
        private readonly ILogger<HealthCheck> _logger;
        private readonly HttpClient _httpClient;

        public HealthCheck(ILogger<HealthCheck> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("APIMgmt");
        }

        [Function("HealthCheck")]
        public async Task<HealthCheckResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Health Check triggered");

            var response = await _httpClient.GetAsync("/status-0123456789abcdef");

            return (response.StatusCode == System.Net.HttpStatusCode.OK) ?
                HealthCheckResult.Healthy() :
                HealthCheckResult.Unhealthy();
        }
    }
}
