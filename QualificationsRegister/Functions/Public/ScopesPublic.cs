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
    public class ScopesPublic
    {
        private readonly ILogger _logger;
        private readonly IGetScopesByOrganisationNumberUseCase _getScopes;

        public ScopesPublic(ILoggerFactory loggerFactory, IGetScopesByOrganisationNumberUseCase getScopes)
        {
            _logger = loggerFactory.CreateLogger<ScopesPublic>();
            _getScopes = getScopes;
        }

        /// <summary>
        /// Returns the list of scopes for an organisation number
        /// </summary>
        /// <param name="req"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [Function("Scopes")]
        public async Task<HttpResponseData> GetScope([HttpTrigger(AuthorizationLevel.Function, "get",
            Route = "scopes/{organisationNumber}")] HttpRequestData req, string organisationNumber)
        {
            _logger.LogInformation("Get Scope for organisation {}", organisationNumber);

            var response = Utilities.CreateResponse(req);

            try
            {
                var scopes = await _getScopes.GetScopesByOrganisationNumber(organisationNumber);

                if (scopes == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                await response.WriteStringAsync(JsonSerializer.Serialize(scopes, Utilities.JsonSerializerOptions));
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
