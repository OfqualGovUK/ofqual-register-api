using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Database;
using Ofqual.Common.RegisterAPI.Mappers;
using Ofqual.Common.RegisterAPI.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ofqual.Common.RegisterAPI.Services.Database
{
    public partial class RegisterDb : IRegisterDb
    {
        private RegisterDbContext _context;
        private ILogger _logger;

        public RegisterDb(RegisterDbContext registerDbContext, ILoggerFactory loggerFactory)
        {
            _context = registerDbContext;
            _logger = loggerFactory.CreateLogger<RegisterDb>();
        }

        public (List<Organisation>?, int count) GetOrganisationsList(int limit, int offSet, string name)
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
               .Skip(offSet)
               .Take(limit)
               .ToList();

            return (organisations?.ToDomain(), count);
        }

        public Organisation? GetOrganisationByNumber(string number, string numberRN)
        {
            _logger.LogInformation($"Getting an organisation by number: {numberRN} or {number}");
            var organisation = _context.Organisations.FirstOrDefault(o => o.RecognitionNumber.Equals(number) ||
            o.RecognitionNumber.Equals(numberRN));

            return organisation?.ToDomain();
        }


        #region Qualifications Private

        public ListResponse<Qualification> GetQualificationsByName(int page, int limit, QualificationFilter? query, string title = "")
        {
            var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);
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
            var list = filteredList.Skip((page - 1) * limit).Take(limit);

            return new ListResponse<Qualification>
            {
                Count = count,
                CurrentPage = page,
                Limit = limit,
                Results = list.ToDomain()
            };
        }

        public Qualification? GetQualificationByNumber(string numberObliques = "", string numberNoObliques = "")
        {
            var quals = _context.Qualifications.OrderBy(e => e.QualificationNumber);

            if (!string.IsNullOrEmpty(numberObliques))
            {
                return quals.FirstOrDefault(e => e.QualificationNumber.Equals(numberObliques))?.ToDomain();
            }

            //add implied obliques in case no obliques value in the db is null
            var qualNumObliques = numberNoObliques.Insert(3, "/").Insert(8, "/");
            return quals.FirstOrDefault(e => e.QualificationNumber.Equals(qualNumObliques)
                                    || (e.QualificationNumberNoObliques != null &&
                                        e.QualificationNumberNoObliques.Equals(numberNoObliques)))?.ToDomain();
        }
        #endregion

        #region Qualifications Public
        public ListResponse<QualificationPublic> GetQualificationsPublicByName(int page, int limit, QualificationFilter? query, string title = "")
        {
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
            var list = filteredList.Skip((page - 1) * limit).Take(limit);

            return new ListResponse<QualificationPublic>
            {
                Count = count,
                CurrentPage = page,
                Limit = limit,
                Results = list.ToDomain()
            };
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
            var qualNumObliques = numberNoObliques.Insert(3, "/").Insert(8, "/");
            return quals.FirstOrDefault(e => e.QualificationNumber.Equals(qualNumObliques)
                                    || (e.QualificationNumberNoObliques != null &&
                                        e.QualificationNumberNoObliques.Equals(numberNoObliques)))?.ToDomain();

        }

        #endregion
    }
}
