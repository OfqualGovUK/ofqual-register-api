using Microsoft.Azure.Functions.Worker.Http;
using Ofqual.Common.RegisterAPI.Constants;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using static Ofqual.Common.RegisterAPI.Constants.SearchConstants;

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

        public static List<string> TokenizeSearchString(string search)
        {
            var tokens = new HashSet<string>();

            search = search.ToLower();

            foreach (var item in Tokens.Keys)
            {
                if (search.Contains(item))
                {
                    tokens.Add(Tokens[item]);

                    search = search.Replace(item, string.Empty);
                }
            }

            foreach (var item in SplitLimiters)
            {
                if (search.Contains(item))
                {
                    search = search.Replace(item, " ");
                }
            }

            var searchTerms = search.Split(" ").ToList();

            foreach (var item in StopWords)
            {
                if (searchTerms.Contains(item))
                {
                    searchTerms.Remove(item);
                }
            }

            tokens.UnionWith(searchTerms);

            return tokens.ToList();
        }
    }
}
