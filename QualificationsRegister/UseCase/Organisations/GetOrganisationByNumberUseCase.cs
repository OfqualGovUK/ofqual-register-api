using Azure;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Net;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;

        public GetOrganisationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByNumberUseCase>();
            _registerDb = registerDb;
        }

        public Organisation? GetOrganisationByNumber(string? number)
        {
            if (string.IsNullOrEmpty(number))
            {
            }

            string numberRN = string.Empty, numberNoRN = string.Empty;

            if (Regex.IsMatch(number!, @"^\d+$"))
            {
                numberNoRN = number!;
                numberRN = $"RN{number}";
            }

            if (number!.Substring(0, 2).ToLower().Equals("rn"))
            {
                numberNoRN = number[2..];
                numberRN = number;
            }

            return _registerDb.GetOrganisationByNumber(numberNoRN, numberRN);
        }

    }
}
