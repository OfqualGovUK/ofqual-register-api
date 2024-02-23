using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Qualifications
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

        [Function("QualificationsListPrivate")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> GetListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string search = "")
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var x = await _getQualifications.GetQualifications(search);
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }


        [Function("QualificationPrivate")]
        //Returns a single qualification based on the id parameter provided in the HttpRequestData
        public async Task<HttpResponseData> GetQualification([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string number)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var x = await _getQualificationByNumber.GetQualificationByNumber(number);
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }
    }
}
