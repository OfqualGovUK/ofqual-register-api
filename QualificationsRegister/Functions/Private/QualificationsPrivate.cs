using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Private
{
    public class QualificationsPrivate
    {
        private readonly ILogger _logger;
        private readonly IGetQualificationsUseCase _getQualifications;
        private readonly IGetQualificationByNumberUseCase _getQualificationByNumber;

        public QualificationsPrivate(ILoggerFactory loggerFactory, IGetQualificationsUseCase searchQualifications,
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
        public async Task<HttpResponseData> GetListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string search = "")
        {
            _logger.LogInformation("Get Qualifications Private - search = {}", search);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var qualifications = await _getQualifications.GetQualificationsPrivate(search);
            await response.WriteStringAsync(JsonSerializer.Serialize(qualifications));

            return response;
        }

        /// <summary>
        /// Returns a single qualification based on the number param
        /// </summary>
        /// <param name="req"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        [Function("QualificationPrivate")]
        public async Task<HttpResponseData> GetQualification([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string number = "")
        {
            _logger.LogInformation("Get Qualification Private - number = {}", number);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (string.IsNullOrEmpty(number))
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                return response;
            }

            var qualification = await _getQualificationByNumber.GetQualificationByNumberPrivate(number);

            if (qualification == null)
            {
                response = req.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }

            await response.WriteStringAsync(JsonSerializer.Serialize(qualification));

            return response;
        }
    }
}
