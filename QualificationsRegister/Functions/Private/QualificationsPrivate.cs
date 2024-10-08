using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Private
{
    public class QualificationsPrivate
    {
        private readonly ILogger _logger;
        private readonly IGetQualificationsListUseCase _getQualifications;
        private readonly IGetQualificationByNumberUseCase _getQualificationByNumber;

        public QualificationsPrivate(ILoggerFactory loggerFactory, IGetQualificationsListUseCase searchQualifications,
            IGetQualificationByNumberUseCase getQualificationByNumber)
        {
            _logger = loggerFactory.CreateLogger<QualificationsPrivate>();
            _getQualifications = searchQualifications;
            _getQualificationByNumber = getQualificationByNumber;
        }

        /// <summary>
        /// Returns the list of qualifications based on the search params (if any)
        /// </summary>
        /// <param name="req"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [Function("QualificationsPrivate")]
        public async Task<HttpResponseData> ListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, int? limit, int page = 1, string title = "")
        {
            _logger.LogInformation("Get Qualifications Private - search = {}", title);

            var response = Utilities.CreateResponse(req);

            try
            {
                var query = req.Query?.GetQualificationFilterQuery();

                var qualifications = _getQualifications.ListQualifications(page, limit, query, title);
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
        [Function("QualificationPrivate")]
        public async Task<HttpResponseData> GetQualification([HttpTrigger(AuthorizationLevel.Function, "get", Route = "Qualificationsprivate/{number}/{number2?}/{number3?}")] HttpRequestData req, string number, string? number2, string? number3)
        {
            _logger.LogInformation("Get Qualification Private - number = {}", number);

            var response = Utilities.CreateResponse(req);

            try
            {
                var qualification = _getQualificationByNumber.GetQualificationByNumber(number, number2, number3);

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
