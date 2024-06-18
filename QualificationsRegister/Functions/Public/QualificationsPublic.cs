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
        public async Task<HttpResponseData> ListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, int? limit = null, int page = 1, string title = "")
        {
            _logger.LogInformation("List Qualifications Public - title = {}", title);

            var response = Utilities.CreateResponse(req);

            try
            {
                var query = req.Query?.GetQualificationFilterQuery();

                //if limit is set to 0, default to getting all quals
                limit = limit == 0 ? null : limit;

                var qualifications = _getQualifications.ListQualificationsPublic(page, limit, query, title);
                _logger.LogInformation("Serializing {} Quals", qualifications.Count);

                await response.WriteStringAsync(JsonSerializer.Serialize(qualifications, Utilities.JsonSerializerOptions));
            }
            catch (Exception ex)
            {
                Utilities.CreateExceptionJson(ex, ref response);
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
                    throw new NotFoundException("Requested Qualification was not found");
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
