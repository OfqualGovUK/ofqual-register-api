using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Private
{
    public class OrganisationsPrivate
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsUseCase _getOrganisations;
        private readonly IGetOrganisationByReferenceUseCase _getOrganisationByReference;

        public OrganisationsPrivate(ILoggerFactory loggerFactory, IGetOrganisationsUseCase getOrganisations,
            IGetOrganisationByReferenceUseCase getOrganisationByReference)
        {
            _logger = loggerFactory.CreateLogger<QualificationsPrivate>();
            _getOrganisations = getOrganisations;
            _getOrganisationByReference = getOrganisationByReference;
        }

        [Function("OrganisationsPrivate")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> GetOrganisationsList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string? search = "")
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var x = await _getOrganisations.GetOrganisations(search);
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }


        [Function("OrganisationPrivate")]
        //Returns a single organisation based on the id parameter provided in the HttpRequestData
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string reference)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var x = await _getOrganisationByReference.GetOrganisationByReference(reference);
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }

    }
}
