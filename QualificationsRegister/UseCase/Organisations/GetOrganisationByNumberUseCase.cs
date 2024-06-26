using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.UseCase.Organisations

{
    public partial class GetOrganisationByNumberUseCase : IGetOrganisationByNumberUseCase
    {

        [GeneratedRegex(@"^\d+$")]
        private static partial Regex OrgNumberRegex();

        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;

        public GetOrganisationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb)
        {
            _logger = loggerFactory.CreateLogger<GetOrganisationByNumberUseCase>();
            _registerDb = registerDb;
        }

        public Organisation? GetOrganisationByNumber(string? number)
        {
            _logger.LogInformation("Getting Organisation by number");

            if (string.IsNullOrEmpty(number))
            {
                throw new BadRequestException("Organisation number is null or empty");
            }

            string numberNoRN, numberRN;

            //match or add RN to the org number if required
            if (OrgNumberRegex().IsMatch(number))
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


            return _registerDb.GetOrganisationByNumber(numberNoRN, numberRN)?.ToDomain();
        }
    }
}
