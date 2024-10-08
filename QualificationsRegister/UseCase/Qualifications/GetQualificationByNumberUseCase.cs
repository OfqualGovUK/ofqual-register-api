using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Exceptions;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Net;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.UseCase.Qualifications
{
    public partial class GetQualificationByNumberUseCase : IGetQualificationByNumberUseCase
    {
        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;

        [GeneratedRegex("\\b\\d{3}\\/\\d{4}\\/\\w\\b")]
        private static partial Regex QualificationNumRegex();

        [GeneratedRegex("\\b\\d{7}\\w\\b")]
        private static partial Regex QualificationNumNoObliquesRegex();

        [GeneratedRegex("[a-zA-Z0-9]{8}")]
        private static partial Regex QualificationNumAlphaNumeric();


        public GetQualificationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationByNumberUseCase>();
            _registerDb = registerDb;
        }

        public QualificationPublic? GetQualificationPublicByNumber(string number, string? number2, string? number3)
        {
            _logger.LogInformation("Getting public qualification by number");

            number = GetQualNumber(number, number2, number3);

            if (QualificationNumRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationPublicByNumber(number, "", null)?.ToDomain();
            }

            if (QualificationNumNoObliquesRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationPublicByNumber("", number, null)?.ToDomain();
            }

            if (QualificationNumAlphaNumeric().IsMatch(number))
            {
                return _registerDb.GetQualificationPublicByNumber("", "", number)?.ToDomain();
            }

            return null;
        }


        public Qualification? GetQualificationByNumber(string number, string? number2, string? number3)
        {
            _logger.LogInformation("Getting private qualification by number");

            number = GetQualNumber(number, number2, number3);

            if (QualificationNumRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationByNumber(number, "", null)?.ToDomain();
            }

            if (QualificationNumNoObliquesRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationByNumber("", number, null)?.ToDomain();

            }

            if (QualificationNumAlphaNumeric().IsMatch(number))
            {
                return _registerDb.GetQualificationByNumber("", "", number)?.ToDomain();

            }

            return null;
        }

        private static string GetQualNumber(string number, string? number2, string? number3)
        {
            if (number2 != null)
            {
                number = number + "/" + number2 + "/" + number3;
            }

            if (string.IsNullOrEmpty(number) || (number2 != null && number3 == null) || (number2 == null && number3 != null))
            {
                throw new BadRequestException("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229");
            }
            else if(!(QualificationNumRegex().IsMatch(number) || QualificationNumNoObliquesRegex().IsMatch(number) || QualificationNumAlphaNumeric().IsMatch(number)))
            {
                throw new BadRequestException("Invalid Qualification number format. Permitted format: 500/1522/9 or 50015229");
            }

            return number;
        }
    }
}
