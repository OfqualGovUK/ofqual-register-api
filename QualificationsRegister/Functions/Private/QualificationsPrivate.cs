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

            try
            {
                var qualifications = await _getQualifications.GetQualificationsPrivate(search);
                _logger.LogInformation("Serializing {} Quals", qualifications.Count);

                response.WriteString(JsonSerializer.Serialize(qualifications));
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
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            try
            {
                var qualification = await _getQualificationByNumber.GetQualificationByNumberPrivate(number);

                if (qualification == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.WriteString(JsonSerializer.Serialize(qualification));
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
    }
}
