using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using Ofqual.Common.RegisterAPI.Models.Public;
using System.Text.Json;
using Ofqual.Common.RegisterAPI.UseCase.Organisations;

namespace Ofqual.Common.RegisterAPI.Functions.Public
{
    public class OrganisationsPublic
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsUseCase _getOrganisations;
        private readonly IGetOrganisationByNumberUseCase _getOrganisationByNumberUseCase;

        public OrganisationsPublic(ILoggerFactory loggerFactory, IGetOrganisationsUseCase getOrganisations,
            IGetOrganisationByNumberUseCase getOrganisationByNumberUseCase)
        {
            _logger = loggerFactory.CreateLogger<QualificationsPublic>();
            _getOrganisations = getOrganisations;
            _getOrganisationByNumberUseCase = getOrganisationByNumberUseCase;
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
                var organisations = await _getOrganisations.GetOrganisations(search);
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
        /// Returns a single organisation based on the number param
        /// Returns 404 if no org is found for the number provided
        /// </summary>
        /// <param name="req"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [Function("Organisation")]
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string number = "")
        {
            _logger.LogInformation("Get Organisation - Public = {}", number);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (string.IsNullOrEmpty(number))
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            try
            {
                var organisation = await _getOrganisationByNumberUseCase.GetOrganisationByNumber(number);

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
