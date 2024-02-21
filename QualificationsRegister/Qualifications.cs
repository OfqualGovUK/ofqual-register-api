using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions
{
    public class Qualifications
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsSearchUseCase _searchOrganisations;
        private readonly IGetOrganisationByNumberUseCase _getOrganisationByNumber;

        public Qualifications(ILoggerFactory loggerFactory, IGetOrganisationsSearchUseCase searchOrganisations,
            IGetOrganisationByNumberUseCase getOrganisationByNumber)
        {
            _logger = loggerFactory.CreateLogger<Qualifications>();
            //_searchOrganisations = searchOrganisations;
            _getOrganisationByNumber = getOrganisationByNumber;
        }

        [Function("Qualifications")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> ListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string x = await _getOrganisationByNumber.GetOrganisationByNumber(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }


        [Function("Qualification")]
        //Returns a single qualification based on the id parameter provided in the HttpRequestData
        public async Task<HttpResponseData> GetQualification([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string x = await _getOrganisationByNumber.GetOrganisationByNumber(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }
    }
}
