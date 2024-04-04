using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {
        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;
        private readonly string _apiUrl;

        public GetOrganisationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb, ApiOptions options)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByNumberUseCase>();
            _registerDb = registerDb;
            _apiUrl = options.ApiUrl;
        }

        public Organisation? GetOrganisationByNumber(string? number)
        {
            if (string.IsNullOrEmpty(number))
            {
                throw new BadRequestException("Organisation number is null or empty");
            }

            string numberNoRN, numberRN;

            if (Regex.IsMatch(number!, @"^\d+$"))
            {
                numberNoRN = number!;
                numberRN = $"RN{number}";
            }
            else if (number!.Substring(0, 2).Equals("rn", StringComparison.InvariantCultureIgnoreCase))
            {
                numberNoRN = number[2..];
                numberRN = number;
            }
            else
                throw new BadRequestException("Please provide a valid organisation number");

            _logger.LogInformation($"Getting organisation: {numberRN}, {numberNoRN}");
            var organisation = _registerDb.GetOrganisationByNumber(numberNoRN, numberRN);
            return organisation?.ToResponse(_apiUrl);
        }

    }
}
