using System.Net;
using System.Text.Json;
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
            _logger = loggerFactory.CreateLogger<OrganisationsPrivate>();
            _getOrganisations = getOrganisations;
            _getOrganisationByReference = getOrganisationByReference;
        }

        /// <summary>
        /// Returns the list of organisations based on the search param
        /// </summary>
        /// <param name="req"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [Function("OrganisationsPrivate")]
        public async Task<HttpResponseData> GetOrganisationsList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string search = "")
        {
            _logger.LogInformation("Get Organisations Private - search = {}", search);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var organisations = await _getOrganisations.GetOrganisations(search);
            await response.WriteStringAsync(JsonSerializer.Serialize(organisations));

            return response;
        }

        /// <summary>
        /// Returns a single organisation based on the reference param
        /// Returns 404 if no org is found for the reference provided
        /// </summary>
        /// <param name="req"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        [Function("OrganisationPrivate")]
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string reference = "")
        {
            _logger.LogInformation("Get Organisation Private - reference = {}", reference);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (string.IsNullOrEmpty(reference))
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);                
                return response;
            }

            var organisation = await _getOrganisationByReference.GetOrganisationByReference(reference);

            if (organisation == null)
            {
                response = req.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }

            await response.WriteStringAsync(JsonSerializer.Serialize((organisation)));

            return response;
        }

    }
}
