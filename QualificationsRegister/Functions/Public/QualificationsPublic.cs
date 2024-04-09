using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Net;
using System.Text.Json;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models.Exceptions;


namespace Ofqual.Common.RegisterAPI.Functions.Public
{
    public class QualificationsPublic
    {
        private readonly ILogger _logger;
        private readonly IGetQualificationsListUseCase _getQualifications;
        private readonly IGetQualificationByNumberUseCase _getQualificationByNumber;

        public QualificationsPublic(ILoggerFactory loggerFactory, IGetQualificationsListUseCase searchQualifications,
            IGetQualificationByNumberUseCase getQualificationByNumber)
        {
            _logger = loggerFactory.CreateLogger<QualificationsPublic>();
            _getQualifications = searchQualifications;
            _getQualificationByNumber = getQualificationByNumber;
        }

        /// <summary>
        /// Returns the list of qualifications based on the search params (if any)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        [Function("Qualifications")]
        public async Task<HttpResponseData> ListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, int page = 1, int limit = 50, string title = "")
        {
            _logger.LogInformation("List Qualifications Public - title = {}", title);

            var response = Utilities.CreateResponse(req);

            if (page < 1 || limit > 100 || limit < 1)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    error = "Invalid parameter values. Page should be > 0 and Limit should be > 0 and <= 100"
                }, Utilities.JsonSerializerOptions));

                return response;
            }

            try
            {
                var query = req.Query == null ? null : req.Query.GetQualificationFilterQuery();

                var qualifications = _getQualifications.ListQualificationsPublic(page, limit, query, title);
                _logger.LogInformation("Serializing {} Quals", qualifications.Count);

                await response.WriteStringAsync(JsonSerializer.Serialize(qualifications, Utilities.JsonSerializerOptions));
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    error = ex.Message,
                    innerException = ex.InnerException
                }, Utilities.JsonSerializerOptions));
            }

            return response;
        }

        /// <summary>
        /// Returns a single qualification based on the number param
        /// </summary>
        /// <param name="req"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [Function("Qualification")]
        public async Task<HttpResponseData> GetQualification([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Qualifications/{number}/{number2?}/{number3?}")] HttpRequestData req, string number, string? number2, string? number3)
        {
            _logger.LogInformation("Get Qualification Public - number = {}", number);

            var response = Utilities.CreateResponse(req);

            try
            {
                var qualification = _getQualificationByNumber.GetQualificationPublicByNumber(number, number2, number3);

                if (qualification == null)
                {
                    throw new NotFoundException(null);
                    return response;
                }

                await response.WriteStringAsync(JsonSerializer.Serialize(qualification, Utilities.JsonSerializerOptions));
            }
            catch (Exception ex)
            {
                Utilities.CreateExceptionJson(ex, ref response);                
            }

            return response;
        }
    }
}
