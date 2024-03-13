using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public async Task<List<Organisation>> GetOrganisations()
        {
            return await _context.Organisations.ToListAsync();
        }

        public async Task<List<Qualification>> GetQualifications(string search = "")
        {
            var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);

            if (string.IsNullOrEmpty(search))
            {
                return await quals.ToListAsync();
            }

            //check the qualification number (obliques)
            var regexMatch = QualificationNumRegex().Match(search);

            //if a search was done for qualification number with obliques, search by qualification number
            if (regexMatch.Success)
            {
                return await quals.Where(e => e.QualificationNumber.Equals(search)).ToListAsync();
            }

            //check the qualification number (with obliques)
            var regexMatchNoObliques = QualificationNumNoObliquesRegex().Match(search);

            //if a search was done for qualification number without obliques, search by qualification number
            if (regexMatchNoObliques.Success)
            {
                var qualNumObliques = search.Insert(3, "/").Insert(8, "/");
                return await quals.Where(e => e.QualificationNumber.Equals(qualNumObliques)
                                        || (e.QualificationNumberNoObliques != null &&
                                            e.QualificationNumberNoObliques.Equals(search)))
                                        .ToListAsync();
            }

            return await quals.Where(e => e.Title.Contains(search)).ToListAsync();
        }



        public QualificationMetaData GetQualificationMeta(List<Qualification> qualifications)
        {
            var meta = new QualificationMetaData
            {
                AssessmentMethod = qualifications.SelectMany(c => c.AssessmentMethods ?? Array.Empty<string>()).GroupWithCount(),
                AssessmentType = qualifications.Select(c => c.Type ?? "").GroupWithCount(),
                AwardingOrganision = qualifications.Select(c => c.OrganisationName).GroupWithCount(),
                Availability = qualifications.Select(c => c.Status ?? "").GroupWithCount(),
                GradingType = qualifications.Select(c => c.GradingType ?? "").GroupWithCount(),
                NationalAvailability = new[]
                {
                    new KeyValuePair<string, int>("England", qualifications.Count(p => p.OfferedInEngland == true)),
                    new KeyValuePair<string, int>("Northern Ireland", qualifications.Count(p => p.OfferedInNorthernIreland == true)),
                    new KeyValuePair<string, int>("International", qualifications.Count(p => p.OfferedInternationally == true))
                }.ToDictionary(),
                QualificationType = qualifications.Select(c => c.Type ?? "").GroupWithCount(),
                QualificationLevel = qualifications.Select(c => c.Level ?? "").GroupWithCount(),
                QualificationSubLevel = qualifications.Select(c => c.SubLevel ?? "").GroupWithCount(),
                SectorSubjectArea = qualifications.Select(c => c.SSA ?? "").GroupWithCount(),
                MinGuidedLearningHours = qualifications.Select(c => (c.MinimumGLH == 1 ? "1 Hour" : $"{c.MinimumGLH} Hours") ?? "None").GroupWithCount(),
                MaxGuidedLearningHours = qualifications.Select(c => (c.MaximumGLH == 1 ? "1 Hour" : $"{c.MaximumGLH} Hours") ?? "None").GroupWithCount(),
                MinTotalQualificationTime = qualifications.Select(c => (c.TQT == 1 ? "1 Hour" : $"{c.TQT} Hours") ?? "None").GroupWithCount(),
                MaxTotalQualificationTime = qualifications.Select(c => (c.TQT == 1 ? "1 Hour" : $"{c.TQT} Hours") ?? "None").GroupWithCount()
            };

            return meta;
        }

        public async Task<List<QualificationPublic>> GetQualificationsPublic(string search = "")
        {
            var quals = _context.QualificationsPublic.OrderBy(e => e.QualificationNumber);

            if (string.IsNullOrEmpty(search))
            {
                return await quals.ToListAsync();
            }

            //check the qualification number (obliques)
            var regexMatch = QualificationNumRegex().Match(search);

            //if a search was done for qualification number with obliques, search by qualification number
            if (regexMatch.Success)
            {
                return await quals.Where(e => e.QualificationNumber.Equals(search)).ToListAsync();
            }

            //check the qualification number (with obliques)
            var regexMatchNoObliques = QualificationNumNoObliquesRegex().Match(search);

            //if a search was done for qualification number without obliques, search by qualification number
            if (regexMatchNoObliques.Success)
            {
                var qualNumObliques = search.Insert(3, "/").Insert(8, "/");
                return await quals.Where(e => e.QualificationNumber.Equals(qualNumObliques)
                                        || (e.QualificationNumberNoObliques != null &&
                                            e.QualificationNumberNoObliques.Equals(search)))
                                        .ToListAsync();
            }

            return await quals.Where(e => e.Title.Contains(search)).ToListAsync();
        }

    }
}
