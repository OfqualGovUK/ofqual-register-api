using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Organisations
{
    public class Organisations
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsUseCase _searchOrganisations;
        private readonly IGetOrganisationByReferenceUseCase _GetOrganisationByReference;

        public Organisations(ILoggerFactory loggerFactory, IGetOrganisationsUseCase searchOrganisations,
            IGetOrganisationByReferenceUseCase GetOrganisationByReference) : base()
        {
            _logger = loggerFactory.CreateLogger<Organisations>();
            _searchOrganisations = searchOrganisations;
            _GetOrganisationByReference = GetOrganisationByReference;
        }

        [Function("Organisations")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> ListOrganisations([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var x = await _GetOrganisationByReference.GetOrganisationByReference(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }


        [Function("Organisation")]
        //Returns a single qualification based on the id parameter provided in the HttpRequestData
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var x = await _GetOrganisationByReference.GetOrganisationByReference(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }

    }
}
