using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ofqual.Common.RegisterAPI.Services.Database
{
    public class RegisterDb : IRegisterDb
    {
        private readonly RegisterDbContext _context;
        private readonly ILogger _logger;

        public RegisterDb(RegisterDbContext registerDbContext, ILoggerFactory loggerFactory)
        {
            _context = registerDbContext;
            _logger = loggerFactory.CreateLogger<RegisterDb>();
        }

        public DbListResponse<DbOrganisation> GetOrganisationsList(int page, int? limit, string name)
        {
            var nameSearchPattern = $"%{name?.Replace(" ", "")}%";

            _logger.LogInformation($"Getting list of organisations: {name}");

            var result = _context.Organisations.Where(o =>
            EF.Functions.Like(o.Acronym.Replace(" ", ""), nameSearchPattern) ||
            EF.Functions.Like(o.LegalName.Replace(" ", ""), nameSearchPattern));

            int count = result.Count();
            var organisations = result.OrderBy(o => o.Name)
               .ThenBy(o => o.LegalName)
               .ThenBy(o => o.Acronym)
               .Skip(page * (limit ?? 1));

            organisations = limit != null ? organisations.Take(limit.Value) : organisations;

            return new DbListResponse<DbOrganisation>
            {
                Count = count,
                Results = organisations.ToList()
            };
        }

        public DbOrganisation? GetOrganisationByNumber(string number, string numberRN)
        {
            _logger.LogInformation($"Getting an organisation by number: {numberRN} or {number}");
            var organisation = _context.Organisations.FirstOrDefault(o => o.RecognitionNumber.Equals(number) ||
            o.RecognitionNumber.Equals(numberRN));

            return organisation;
        }


        #region Qualifications Private

        public DbListResponse<DbQualification> GetQualificationsList(int page, int limit, QualificationFilter? query, string title)
        {
            _logger.LogInformation($"Getting a qualification by title: {title}");

            var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);
            var count = 0;

            //to initialise the IQueryable
            var filteredList = quals.Where(e => true);

            if (!string.IsNullOrEmpty(title))
            {
                filteredList = filteredList.Where(q => q.Title.Contains(title));
            }

            if (query != null)
            {
                if (query.AssessmentMethods != null)
                {
                    foreach (var aM in query.AssessmentMethods)
                    {
                        filteredList = filteredList.Where(q => q.AssessmentMethods!.Contains(aM));
                    }
                }

                if (query.AwardingOrganisations != null)
                {
                    foreach (var aO in query.AwardingOrganisations)
                    {
                        filteredList = filteredList.Where(q => q.OrganisationName == aO);
                    }
                }

                if (query.Availability != null)
                {
                    foreach (var av in query.Availability)
                    {
                        filteredList = filteredList.Where(q => q.Status == av);
                    }
                }

                if (query.QualificationTypes != null)
                {
                    foreach (var qT in query.QualificationTypes)
                    {
                        filteredList = filteredList.Where(q => q.Type == qT);
                    }
                }

                if (query.QualificationLevels != null)
                {
                    foreach (var qL in query.QualificationLevels)
                    {
                        filteredList = filteredList.Where(q => q.Level == qL);
                    }
                }

                if (query.QualificationSubLevels != null)
                {
                    foreach (var qSL in query.QualificationSubLevels)
                    {
                        filteredList = filteredList.Where(q => q.SubLevel == qSL);
                    }
                }

                if (query.NationalAvailability != null)
                {
                    foreach (var nA in query.NationalAvailability)
                    {
                        switch (nA.ToLower().Trim())
                        {
                            case "england":
                                filteredList = filteredList.Where(q => q.OfferedInEngland == true);
                                break;
                            case "northern ireland":
                                filteredList = filteredList.Where(q => q.OfferedInNorthernIreland == true);
                                break;
                            case "international":
                                filteredList = filteredList.Where(q => q.OfferedInternationally == true);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (query.MinGuidedLearninghours != null)
                {
                    filteredList = filteredList.Where(q => q.GLH >= query.MinGuidedLearninghours);
                }

                if (query.MaxGuidedLearninghours != null)
                {
                    filteredList = filteredList.Where(q => q.GLH <= query.MaxGuidedLearninghours);
                }

                if (query.MinTotalQualificationTime != null)
                {
                    filteredList = filteredList.Where(q => q.TQT >= query.MinTotalQualificationTime);
                }

                if (query.MaxTotalQualificationTime != null)
                {
                    filteredList = filteredList.Where(q => q.TQT >= query.MaxTotalQualificationTime);
                }

                if (query.SectorSubjectAreas != null)
                {
                    foreach (var sSA in query.SectorSubjectAreas)
                    {
                        filteredList = filteredList.Where(q => q.SSA == sSA);
                    }
                }
            }

            count = filteredList.Count();
            var list = filteredList.Skip(page * limit).Take(limit);

            return new DbListResponse<DbQualification>
            {
                Count = count,
                Results = list.ToList()
            };
        }

        public DbQualification? GetQualificationByNumber(string numberObliques, string numberNoObliques)
        {
            _logger.LogInformation($"Getting a qualification by number: {numberObliques} or {numberNoObliques}");
            var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);

            if (!string.IsNullOrEmpty(numberObliques))
            {
                return quals.FirstOrDefault(e => e.QualificationNumber.Equals(numberObliques));
            }

            //add implied obliques in case no obliques value in the db is null
            var qualNumObliques = numberNoObliques.Insert(3, "/").Insert(8, "/");
            return quals.FirstOrDefault(e => e.QualificationNumber.Equals(qualNumObliques)
                                    || (e.QualificationNumberNoObliques != null &&
                                        e.QualificationNumberNoObliques.Equals(numberNoObliques)));
        }
        #endregion

        #region Qualifications Public
        public DbListResponse<DbQualificationPublic> GetQualificationsPublicList(int page, int limit, QualificationFilter? query, string title)
        {
            _logger.LogInformation($"Getting a qualification by title: {title}");
            var quals = _context.QualificationsPublic.OrderBy(e => e.QualificationNumber);
            var count = 0;

            //to initialise the IQueryable
            var filteredList = quals.Where(e => e.Id != null);

            if (!string.IsNullOrEmpty(title))
            {
                filteredList = filteredList.Where(q => q.Title.Contains(title));
            }

            if (query != null)
            {
                if (query.AssessmentMethods != null)
                {
                    foreach (var aM in query.AssessmentMethods)
                    {
                        filteredList = filteredList.Where(q => q.AssessmentMethods!.Contains(aM));
                    }
                }

                if (query.AwardingOrganisations != null)
                {
                    foreach (var aO in query.AwardingOrganisations)
                    {
                        filteredList = filteredList.Where(q => q.OrganisationName == aO);
                    }
                }

                if (query.Availability != null)
                {
                    foreach (var av in query.Availability)
                    {
                        filteredList = filteredList.Where(q => q.Status == av);
                    }
                }

                if (query.QualificationTypes != null)
                {
                    foreach (var qT in query.QualificationTypes)
                    {
                        filteredList = filteredList.Where(q => q.Type == qT);
                    }
                }

                if (query.QualificationLevels != null)
                {
                    foreach (var qL in query.QualificationLevels)
                    {
                        filteredList = filteredList.Where(q => q.Level == qL);
                    }
                }

                if (query.QualificationSubLevels != null)
                {
                    foreach (var qSL in query.QualificationSubLevels)
                    {
                        filteredList = filteredList.Where(q => q.SubLevel == qSL);
                    }
                }

                if (query.NationalAvailability != null)
                {
                    foreach (var nA in query.NationalAvailability)
                    {
                        switch (nA.ToLower().Trim())
                        {
                            case "england":
                                filteredList = filteredList.Where(q => q.OfferedInEngland == true);
                                break;
                            case "northern ireland":
                                filteredList = filteredList.Where(q => q.OfferedInNorthernIreland == true);
                                break;
                            case "international":
                                filteredList = filteredList.Where(q => q.OfferedInternationally == true);
                                break;
                            default:
                                break;
                        }
                    }
                }

                if (query.MinGuidedLearninghours != null)
                {
                    filteredList = filteredList.Where(q => q.GLH >= query.MinGuidedLearninghours);
                }

                if (query.MaxGuidedLearninghours != null)
                {
                    filteredList = filteredList.Where(q => q.GLH <= query.MaxGuidedLearninghours);
                }

                if (query.MinTotalQualificationTime != null)
                {
                    filteredList = filteredList.Where(q => q.TQT >= query.MinTotalQualificationTime);
                }

                if (query.MaxTotalQualificationTime != null)
                {
                    filteredList = filteredList.Where(q => q.TQT >= query.MaxTotalQualificationTime);
                }

                if (query.SectorSubjectAreas != null)
                {
                    foreach (var sSA in query.SectorSubjectAreas)
                    {
                        filteredList = filteredList.Where(q => q.SSA == sSA);
                    }
                }
            }

            count = filteredList.Count();
            var list = filteredList.Skip(page * limit).Take(limit);

            return new DbListResponse<DbQualificationPublic>
            {
                Count = count,
                Results = list.ToList()
            };
        }

        public DbQualificationPublic? GetQualificationPublicByNumber(string numberObliques, string numberNoObliques)
        {
            _logger.LogInformation($"Getting a qualification by number: {numberObliques} or {numberNoObliques}");
            var quals = _context.QualificationsPublic.OrderBy(e => e.QualificationNumber);

            //if a search was done for qualification number with obliques, search by qualification number
            if (!string.IsNullOrEmpty(numberObliques))
            {
                return quals.FirstOrDefault(e => e.QualificationNumber.Equals(numberObliques));
            }

            //add implied obliques in case no obliques value in the db is null
            var qualNumObliques = numberNoObliques.Insert(3, "/").Insert(8, "/");
            return quals.FirstOrDefault(e => e.QualificationNumber.Equals(qualNumObliques)
                                    || (e.QualificationNumberNoObliques != null &&
                                        e.QualificationNumberNoObliques.Equals(numberNoObliques)));

        }

        #endregion
    }
}
