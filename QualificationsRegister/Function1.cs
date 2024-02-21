using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using QualificationsRegister.UseCase.Interfaces;

namespace QualificationsRegister.Functions
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsSearchUseCase _searchOrganisations;
        private readonly IGetOrganisationByNumberUseCase _getOrganisationByNumber;

        public Function1(ILoggerFactory loggerFactory, IGetOrganisationsSearchUseCase searchOrganisations,
            IGetOrganisationByNumberUseCase getOrganisationByNumber)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _searchOrganisations = searchOrganisations;
            _getOrganisationByNumber = getOrganisationByNumber;
        }

        [Function("Function1")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string x = await _getOrganisationByNumber.GetOrganisationByNumber(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);
            //IActionResult
            return response;
        }
    }
}
