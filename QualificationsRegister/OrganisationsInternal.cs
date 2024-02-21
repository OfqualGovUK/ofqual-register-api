using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions
{
    public class OrganisationsInternal
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsSearchUseCase _searchOrganisations;
        private readonly IGetOrganisationByNumberUseCase _getOrganisationByNumber;

        public OrganisationsInternal(ILoggerFactory loggerFactory, IGetOrganisationsSearchUseCase searchOrganisations,
            IGetOrganisationByNumberUseCase getOrganisationByNumber)
        {
            _logger = loggerFactory.CreateLogger<QualificationsInternal>();
            //_searchOrganisations = searchOrganisations;
            _getOrganisationByNumber = getOrganisationByNumber;
        }

        [Function("OrganisationsInternal")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> ListOrganisations([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string x = await _getOrganisationByNumber.GetOrganisationByNumber(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }


        [Function("OrganisationInternal")]
        //Returns a single qualification based on the id parameter provided in the HttpRequestData
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string x = await _getOrganisationByNumber.GetOrganisationByNumber(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }

    }
}
