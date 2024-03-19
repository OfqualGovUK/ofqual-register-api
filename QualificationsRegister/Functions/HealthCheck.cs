using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

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
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("Health Check triggered");

            var health = await _httpClient.GetAsync("/status-0123456789abcdef");

            var response = req.CreateResponse(health.StatusCode);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            response.WriteString(JsonSerializer.Serialize((health.StatusCode == HttpStatusCode.OK) ?
                 HealthCheckResult.Healthy() :
                 HealthCheckResult.Unhealthy()));

            return response;
        }
    }
}
