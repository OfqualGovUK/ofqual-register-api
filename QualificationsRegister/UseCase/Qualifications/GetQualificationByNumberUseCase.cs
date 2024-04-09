using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
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


        public GetQualificationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationByNumberUseCase>();
            _registerDb = registerDb;
        }

        public QualificationPublic? GetQualificationPublicByNumber(string number)
        {
            _logger.LogInformation("Getting public qualification by number");
            if (QualificationNumRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationPublicByNumber(number, "")?.ToDomain();
            }

            if (QualificationNumNoObliquesRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationPublicByNumber("", number)?.ToDomain();
            }

            return null;
        }

        public Qualification? GetQualificationByNumber(string number)
        {
            _logger.LogInformation("Getting private qualification by number");

            if (QualificationNumRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationByNumber(number, "")?.ToDomain();
            }

            if (QualificationNumNoObliquesRegex().IsMatch(number))
            {
                return _registerDb.GetQualificationByNumber("", number)?.ToDomain();

            }

            return null;
        }

    }
}
