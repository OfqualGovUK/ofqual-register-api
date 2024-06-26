using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Net;
using System.Text.Json;

namespace Ofqual.Common.RegisterAPI.Functions.Public
{
    public class OrganisationsPublic
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsListUseCase _getOrganisations;
        private readonly IGetOrganisationByNumberUseCase _getOrganisationByNumberUseCase;

        public OrganisationsPublic(ILoggerFactory loggerFactory, IGetOrganisationsListUseCase getOrganisations,
            IGetOrganisationByNumberUseCase getOrganisationByNumberUseCase)
        {
            _logger = loggerFactory.CreateLogger<OrganisationsPublic>();
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
        public async Task<HttpResponseData> GetOrganisationsList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req,
            string? search, int? limit, int page = 1)
        {
            _logger.LogInformation("Get Organisations Public - search = {}", search);

            var response = Utilities.CreateResponse(req);

            try
            {
                var organisations = _getOrganisations.ListOrganisations(search, limit, page);
                await response.WriteStringAsync(JsonSerializer.Serialize(organisations, Utilities.JsonSerializerOptions));
            }
            catch (Exception ex)
            {
                Utilities.CreateExceptionJson(ex, ref response);
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
        public async Task<HttpResponseData> GetOrganisation([HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "organisations/{number}")] HttpRequestData req, string number)
        {
            _logger.LogInformation("Get Organisation - Public = {}", number);

            var response = Utilities.CreateResponse(req);

            try
            {
                var organisation = _getOrganisationByNumberUseCase.GetOrganisationByNumber(number);

                if (organisation == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                await response.WriteStringAsync(JsonSerializer.Serialize(organisation, Utilities.JsonSerializerOptions));
            }
            catch (BadRequestException ex)
            {
                var error = req.CreateResponse(HttpStatusCode.BadRequest);
                error.WriteString(JsonSerializer.Serialize(new
                {
                    error = ex.Message
                }, Utilities.JsonSerializerOptions));
                return error;
            }
            catch (Exception ex)
            {
                Utilities.CreateExceptionJson(ex, ref response);
            }

            return response;
        }

    }
}
