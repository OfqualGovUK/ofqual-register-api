using Dapper;
<<<<<<< HEAD
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Services.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
=======
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Ofqual.Common.RegisterAPI.Models;
>>>>>>> ff67b6ffa7762dd8234da78187e62c48e0ae65d7

namespace Ofqual.Common.RegisterAPI.Services.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
<<<<<<< HEAD
        private readonly IDapperDbConnection _dapperDbConnection;
        private readonly ILogger _logger;

        private List<Organisation> _organisationList;
        private List<Qualification> _qualificationsList;

        public RegisterRepository(IDapperDbConnection dapperDbConnection, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RegisterRepository>();
            _dapperDbConnection = dapperDbConnection;
        }

        public async Task GetOrganisations()
        {
            using (var db = _dapperDbConnection.CreateConnection())
=======
        private RegisterDbContext _databaseContext;
        public RegisterRepository(RegisterDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Organisation>> GetAllOrganisationsAsync()
        {
            using (var db = new SqlConnection(_databaseContext.Database.GetDbConnection().ConnectionString))
>>>>>>> ff67b6ffa7762dd8234da78187e62c48e0ae65d7
            {
                try
                {
                    var organisations =
                     await db.QueryAsync<Organisation>
                        (@"SELECT [Id]
                            ,[Name]
                            ,[RecognitionNumber]
                            ,[LegalName]
                            ,[Acronym]
                            ,[Ofqual_OrganisationStatus] AS OfqualOrganisationStatus
                            ,[CCEA_OrganisationStatus] AS CceaOrganisationStatus
                            ,[Ofqual_RecognisedOn] AS OfqualRecognisedOn
                            ,[OfqualRecognisedTo] AS OfqualRecognisedTo
                            ,[Ofqual_SurrenderedOn] AS  OfqualSurrenderedOn 
                            ,[Ofqual_WithdrawnOn]   AS    OfqualWithdrawnOn 
                            ,[CCEA_RecognisedOn]    AS    CceaRecognisedOn 
                            ,[CCEA_RecognisedTo]    AS    CceaRecognisedTo 
                            ,[CCEA_SurrenderedOn]   AS    CceaSurrenderedOn 
                            ,[CCEA_WithdrawnOn]     AS    CceaWithdrawnOn 
                            ,[ContactEmail]
                            ,[Website]
                            ,[PhoneNumber]
                            ,[FeesUrl]
                            ,[AddressLine1]
                            ,[AddressLine2]
                            ,[AddressCity]
                            ,[AddressCounty]
                            ,[AddressCountry]
                            ,[AddressPostCode]
                            ,[LastUpdatedDate]
                    FROM [MD_Register].[Register_V_Organisation]");

<<<<<<< HEAD
                    _logger.Log(LogLevel.Information, "Got Organisations");

                    _organisationList = organisations.ToList();
=======
                    return organisations;
>>>>>>> ff67b6ffa7762dd8234da78187e62c48e0ae65d7
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

<<<<<<< HEAD
        public async Task GetQualifications()
        {
            using (var db = _dapperDbConnection.CreateConnection())
            {
                try
                {
                    var qualifications =
                     await db.QueryAsync<Qualification>
                        (@"SELECT [Id]
                              ,[QualificationNumber]
                              ,[QualifiationNumberNoObliques]
                              ,[Title]
                              ,[Status]
                              ,[OrganisationName]
                              ,[OrganisationAcronym]
                              ,[OrganisationRecognitionNumber]
                              ,[Type]
                              ,[SSA]
                              ,[Level]
                              ,[SubLevel]
                              ,[EQFLevel]
                              ,[GradingType]
                              ,[GradingScale]
                              ,[TotalCredits]
                              ,[TQT]
                              ,[GLH]
                              ,[MinimumGLH]
                              ,[MaximumGLH]
                              ,[RegulationStartDate]
                              ,[OperationalStartDate]
                              ,[OperationalEndDate]
                              ,[CertificationEndDate]
                              ,[ReviewDate]
                              ,[OfferedInEngland]
                              ,[OfferedInNorthernIreland]
                              ,[OfferedInternationally]
                              ,[Specialism]
                              ,[Pathways]
                              ,[AssessmentMethods]
                              ,[ApprovedForDELFundedProgramme]
                              ,[LinkToSpecification]
                              ,[ApprenticeshipStandardReferenceNumber]
                              ,[ApprenticeshipStandardTitle]
                              ,[RegulatedByNorthernIreland]
                              ,[NIDiscountCode]
                              ,[GCESizeEquivalence]
                              ,[GCSESizeEquivalence]
                              ,[EntitlementFrameworkDesignation]
                              ,[LastUpdatedDate]
                              ,[OrganisationId]
                              ,[LevelId]
                              ,[TypeId]
                              ,[SSAId]
                              ,[GradingTypeId]
                              ,[GradingScaleid]
                          FROM [MD_Register].[Register_V_Qualification]");

                    _logger.Log(LogLevel.Information, "Got Qualifications");

                    _qualificationsList = qualifications.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        async Task<Dictionary<string, object>> IRegisterRepository.GetDataAsync()
        {
            _logger.Log(LogLevel.Information, "Getting Data from DB");

            var thread1 = Task.Run(() => GetQualifications());
            var thread2 = Task.Run(() => GetOrganisations());

            Task.WaitAll(thread1, thread2);

            var dict = new Dictionary<string, object>
            {
                { "Qualifications", _qualificationsList },
                {"Organisations", _organisationList }
            };

            return dict;
        }
=======
>>>>>>> ff67b6ffa7762dd8234da78187e62c48e0ae65d7
    }
}
