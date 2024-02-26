using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.Public;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Public
{
    public class QualificationsPublic
    {
        private readonly ILogger _logger;
        private readonly IGetQualificationsUseCase _getQualifications;
        private readonly IGetQualificationByNumberUseCase _getQualificationByNumber;

        public QualificationsPublic(ILoggerFactory loggerFactory, IGetQualificationsUseCase getQualifications,
            IGetQualificationByNumberUseCase getQualificationByNumber)
        {
            _logger = loggerFactory.CreateLogger<QualificationsPublic>();
            _getQualifications = getQualifications;
            _getQualificationByNumber = getQualificationByNumber;
        }

        [Function("Qualifications")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> GetQualificationsList([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req, string? search = "", bool map = true)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            var x = await _getQualifications.GetQualifications(search);

            if (map)
            {
                _logger.LogInformation("start mapping");
                response.WriteString(JsonSerializer.Serialize(x.Select(e => new QualificationPublic(e))));
                _logger.LogInformation("end mapping");
            }
            else
            {
                _logger.LogInformation("start no mapping");
                response.WriteString(JsonSerializer.Serialize(x));
                _logger.LogInformation("end no mapping");
            }



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
