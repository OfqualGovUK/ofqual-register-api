using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;

namespace Ofqual.Common.RegisterAPI.Functions.Qualifications
{
    public class QualificationsInternal
    {
        private readonly ILogger _logger;
        private readonly IGetOrganisationsUseCase _searchOrganisations;
        private readonly IGetOrganisationByReferenceUseCase _getOrganisationByReference;

        public QualificationsInternal(ILoggerFactory loggerFactory, IGetOrganisationsUseCase searchOrganisations,
            IGetOrganisationByReferenceUseCase getOrganisationByReference)
        {
            _logger = loggerFactory.CreateLogger<QualificationsInternal>();
            //_searchOrganisations = searchOrganisations;
            _getOrganisationByReference = getOrganisationByReference;
        }

        [Function("QualificationsInternal")]
        //Returns the list of qualifications
        public async Task<HttpResponseData> ListQualifications([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var x = await _getOrganisationByReference.GetOrganisationByReference(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }


        [Function("QualificationInternal")]
        //Returns a single qualification based on the id parameter provided in the HttpRequestData
        public async Task<HttpResponseData> GetQualification([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {

            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            var x = await _getOrganisationByReference.GetOrganisationByReference(" ofqual");
            response.WriteString("Welcome to Azure Functions!" + x);

            return response;
        }
    }
}
