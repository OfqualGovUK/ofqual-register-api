using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ofqual.Common.RegisterAPI.Services.Database
{
    public partial class RegisterDb : IRegisterDb
    {
        private RegisterDbContext _context;
        private ILogger _logger;

        [GeneratedRegex("\\b[0-9]{3}\\/[0-9]{4}\\/[0-9A-z]\\b")]
        private static partial Regex QualificationNumRegex();

        [GeneratedRegex("\\b[0-9]{7}\\w\\b")]
        private static partial Regex QualificationNumNoObliquesRegex();

        public RegisterDb(RegisterDbContext registerDbContext, ILoggerFactory loggerFactory)
        {
            _context = registerDbContext;
            _logger = loggerFactory.CreateLogger<RegisterDb>();
        }

        public List<Organisation>? GetOrganisationsList(string name)
        {
            var nameSearchPattern = $"%{name?.Replace(" ", "")}%";

            _logger.LogInformation("Getting list of organisations");
            var organisations = _context.Organisations.Where(o =>
            EF.Functions.Like(o.Acronym.Replace(" ", ""), nameSearchPattern) ||
            EF.Functions.Like(o.LegalName.Replace(" ", ""), nameSearchPattern))
                .OrderBy(o => o.LegalName)
                .ToList();

            return organisations?.ToDomain();
        }

        public Organisation? GetOrganisationByNumber(string number, string numberRN)
        {
            _logger.LogInformation("Getting an organisation by number");
            var organisation = _context.Organisations.FirstOrDefault(o => o.RecognitionNumber.Equals(number) ||
            o.RecognitionNumber.Equals(numberRN));

            return organisation?.ToDomain();
        }

        public List<Qualification> GetQualificationsByName(string name = "")
        {
            var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);

            if (!string.IsNullOrEmpty(name))
            {
                return quals.Where(q => EF.Functions.Contains(q.Title, name)).ToDomain();
            }

            return quals.ToDomain();
        }

        public List<QualificationPublic> GetQualificationsPublicByName(string name = "")
        {
            var quals = _context.QualificationsPublic.OrderBy(e => e.QualificationNumber);

            if (!string.IsNullOrEmpty(name))
            {
                return quals.Where(q => EF.Functions.Contains(q.Title, name)).ToDomain();
            }

            return quals.ToDomain();
        }

        //public Qualification? GetQualificationByNumber(string numberObliques = "", string numberNoObliques = "")
        //{
        //    var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);

        //    //check the qualification number (obliques)
        //    var regexMatch = QualificationNumRegex().Match(number);

        //    //if a search was done for qualification number with obliques, search by qualification number
        //    if (!string.IsNullOrEmpty(numberObliques))
        //    {
        //        return quals.FirstOrDefault(e => e.QualificationNumber.Equals(numberObliques));
        //    }

        //    //check the qualification number (with obliques)
        //    var regexMatchNoObliques = QualificationNumNoObliquesRegex().Match(number);

        //    //if a search was done for qualification number without obliques, search by qualification number
        //    if (!string.IsNullOrEmpty(numberNoObliques))
        //    {
        //        var qualNumObliques = numberObliques.Insert(3, "/").Insert(8, "/");
        //        return quals.FirstOrDefault(e => e.QualificationNumber.Equals(qualNumObliques)
        //                                || (e.QualificationNumberNoObliques != null &&
        //                                    e.QualificationNumberNoObliques.Equals(numberNoObliques)));

        //    }
        //}



        public Qualification? GetQualificationByNumber(string numberObliques = "", string numberNoObliques = "")
        {
            var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);

            if (!string.IsNullOrEmpty(numberObliques))
            {
                return quals.FirstOrDefault(e => e.QualificationNumber.Equals(numberObliques))?.ToDomain();
            }

            //add implied obliques in case no obliques value in the db is null
            var qualNumObliques = numberObliques.Insert(3, "/").Insert(8, "/");
            return quals.FirstOrDefault(e => e.QualificationNumber.Equals(qualNumObliques)
                                    || (e.QualificationNumberNoObliques != null &&
                                        e.QualificationNumberNoObliques.Equals(numberNoObliques)))?.ToDomain();


        }

        public QualificationPublic? GetQualificationPublicByNumber(string numberObliques = "", string numberNoObliques = "")
        {
            var quals = _context.QualificationsPublic.OrderBy(e => e.QualificationNumber);

            //if a search was done for qualification number with obliques, search by qualification number
            if (!string.IsNullOrEmpty(numberObliques))
            {
                return quals.FirstOrDefault(e => e.QualificationNumber.Equals(numberObliques))?.ToDomain();
            }

            //add implied obliques in case no obliques value in the db is null

            var qualNumObliques = numberObliques.Insert(3, "/").Insert(8, "/");
            return quals.FirstOrDefault(e => e.QualificationNumber.Equals(qualNumObliques)
                                    || (e.QualificationNumberNoObliques != null &&
                                        e.QualificationNumberNoObliques.Equals(numberNoObliques)))?.ToDomain();

        }
    }
}
