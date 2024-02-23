using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Organisations
{
    public class Organisations
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsUseCase _getOrganisations;
        private readonly IGetOrganisationByReferenceUseCase _getOrganisationByReference;

        public Organisations(ILoggerFactory loggerFactory, IGetOrganisationsUseCase getOrganisations,
            IGetOrganisationByReferenceUseCase getOrganisationByReference) : base()
        {
            _logger = loggerFactory.CreateLogger<Organisations>();
            _getOrganisations = getOrganisations;
            _getOrganisationByReference = getOrganisationByReference;
        }

        [Function("OrganisationsList")]
        //Returns the list of qualifications based on the search params (if any)
        public async Task<HttpResponseData> GetOrganisationsList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string search = "")
        {
            _logger.LogInformation("Http Trigger to get List ");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var organisation = await _getOrganisations.GetOrganisations(search);
            response.WriteString(JsonSerializer.Serialize(organisation));

            return response;
        }


        [Function("Organisation")]
        //Returns a single qualification based on the id parameter provided in the parameters
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string reference)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var organisations = await _getOrganisationByReference.GetOrganisationByReference(reference);
            response.WriteString(JsonSerializer.Serialize(organisations));

            return response;
        }

    }
}
