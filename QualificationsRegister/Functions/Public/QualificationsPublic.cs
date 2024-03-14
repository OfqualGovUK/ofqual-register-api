using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Text.Json;


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
        /// <param name="search"></param>
        /// <returns></returns>
        [Function("Qualifications")]
        public async Task<HttpResponseData> ListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string name = "")
        {
            _logger.LogInformation("List Qualifications Public - name = {}", name);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            try
            {
                var qualifications = _getQualifications.ListQualificationsPublic(name);
                _logger.LogInformation("Serializing {} Quals", qualifications.Count);

                await response.WriteStringAsync(JsonSerializer.Serialize(qualifications));
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync(JsonSerializer.Serialize(new
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
        [Function("Qualification")]
        public async Task<HttpResponseData> GetQualification([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string number = "")
        {
            _logger.LogInformation("Get Qualification Public - number = {}", number);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            if (string.IsNullOrEmpty(number))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            try
            {
                var qualification = _getQualificationByNumber.GetQualificationPublicByNumber(number);

                if (qualification == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                await response.WriteStringAsync(JsonSerializer.Serialize(qualification));
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    error = ex.Message,
                    innerException = ex.InnerException
                }));
            }

            return response;
        }
    }
}
