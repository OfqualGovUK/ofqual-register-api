using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.UseCase.Interfaces;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.UseCase
{
    public class GetQualificationByNumberUseCase : IGetQualificationByNumberUseCase
    {
        private readonly IRegisterDb _registerDb;
        private readonly ILogger _logger;

        private const string QualificationNumRegex = @"\b\d{3}\/\d{4}\/\w\b";
        private const string QualificationNumNoObliquesRegex = @"\b\d{7}\w\b";

        //[GeneratedRegex("\\b\\d{3}\\/\\d{4}\\/\\w\\b")]
        //private static partial Regex QualificationNumRegex();

        //[GeneratedRegex("\\b\\d{7}\\w\\b")]
        //private static partial Regex QualificationNumNoObliquesRegex();

        //[GeneratedRegex("\\b\\d{3}\\/\\d{4}\\/\\w\\b|\\b\\d{7}\\w\\b")]
        //private static partial Regex QualificationNum();

        public GetQualificationByNumberUseCase(ILoggerFactory loggerFactory, IRegisterDb registerDb)
        {
            _logger = loggerFactory.CreateLogger<GetQualificationByNumberUseCase>();
            _registerDb = registerDb;
        }

        public QualificationPublic? GetQualificationPublicByNumber(string number)
        {           
            if (Regex.IsMatch(number, QualificationNumRegex))
            {
                return _registerDb.GetQualificationPublicByNumber(number, "");
            }

            if (Regex.IsMatch(number, QualificationNumNoObliquesRegex))
            {
                return _registerDb.GetQualificationPublicByNumber("", number);

            }

            return null;
        }

        public Qualification? GetQualificationByNumber(string number)
        {
            if (Regex.IsMatch(number, QualificationNumRegex))
            {
                return _registerDb.GetQualificationByNumber(number, "");
            }

            if (Regex.IsMatch(number, QualificationNumNoObliquesRegex))
            {
                return _registerDb.GetQualificationByNumber("", number);

            }

            return null;
        }

    }
}
