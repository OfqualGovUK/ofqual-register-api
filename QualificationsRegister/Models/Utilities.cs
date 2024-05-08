using Microsoft.Azure.Functions.Worker.Http;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Models
{
    public static class Utilities
    {
        public const int DefaultOrganisationPagingLimit = 200;
        public const int DefaultQualificationPagingLimit = 100;

        public static HttpResponseData CreateResponse(HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            return response;
        }

        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static void CreateExceptionJson(Exception ex, ref HttpResponseData response)
        {
            response.StatusCode = ex switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };

            if (ex.Message != null)
            {
                response.WriteString(JsonSerializer.Serialize(new
                {
                    error = ex.Message
                }, JsonSerializerOptions));
            }
        }
    }
}
