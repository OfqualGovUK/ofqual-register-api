using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.Models.Public;
using System.Text.Json;

namespace Ofqual.Common.RegisterAPI.Functions.Public
{
    public class OrganisationsPublic
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsUseCase _getOrganisations;
        private readonly IGetOrganisationByReferenceUseCase _getOrganisationByReference;

        public OrganisationsPublic(ILoggerFactory loggerFactory, IGetOrganisationsUseCase getOrganisations,
            IGetOrganisationByReferenceUseCase getOrganisationByReference)
        {
            _logger = loggerFactory.CreateLogger<QualificationsPublic>();
            _getOrganisations = getOrganisations;
            _getOrganisationByReference = getOrganisationByReference;
        }

        /// <summary>
        /// Returns the list of organisations based on the search param
        /// </summary>
        /// <param name="req"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [Function("Organisations")]
        public async Task<HttpResponseData> GetOrganisationsList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string search = "")
        {
            _logger.LogInformation("Get Organisations Public - search = {}", search);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            try
            {
                var organisations = await _getOrganisations.GetOrganisationsPublic(search);
                response.WriteString(JsonSerializer.Serialize(organisations));
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.WriteString(JsonSerializer.Serialize(new
                {
                    error = ex.Message,
                    innerException = ex.InnerException
                }));
            }

            return response;
        }

        /// <summary>
        /// Returns a single organisation based on the reference param
        /// Returns 404 if no org is found for the reference provided
        /// </summary>
        /// <param name="req"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        [Function("Organisation")]
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string reference = "")
        {
            _logger.LogInformation("Get Organisation - Public = {}", reference);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (string.IsNullOrEmpty(reference))
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            try
            {
                var organisation = await _getOrganisationByReference.GetOrganisationByReference(reference);

                if (organisation == null)
                {
                    response = req.CreateResponse(HttpStatusCode.NotFound);
                    return response;
                }

                await response.WriteStringAsync(JsonSerializer.Serialize(organisation));
            }
            catch (Exception ex)
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.WriteString(JsonSerializer.Serialize(new
                {
                    error = ex.Message,
                    innerException = ex.InnerException
                }));

            }

            return response;
        }

    }
}
