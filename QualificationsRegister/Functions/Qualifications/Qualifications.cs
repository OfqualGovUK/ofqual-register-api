using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Qualifications
{
    public class Qualifications
    {
        private readonly ILogger _logger;
        private readonly IGetQualificationsUseCase _getQualifications;
        private readonly IGetQualificationByNumberUseCase _getQualificationByNumber;

        public Qualifications(ILoggerFactory loggerFactory, IGetQualificationsUseCase getQualifications,
            IGetQualificationByNumberUseCase getQualificationByNumber)
        {
            _logger = loggerFactory.CreateLogger<Qualifications>();
            _getQualifications = getQualifications;
            _getQualificationByNumber = getQualificationByNumber;
        }

        [Function("QualificationsList")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> GetQualificationsList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string? search = "")
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var x = await _getQualifications.GetQualifications(search);
            response.WriteString(JsonSerializer.Serialize(x));

            return response;
        }


        [Function("Qualification")]
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
