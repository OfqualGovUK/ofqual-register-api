using Dapper;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models.Private;
using Ofqual.Common.RegisterAPI.Services.Data;


namespace Ofqual.Common.RegisterAPI.Services.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly IDapperDbConnection _dapperDbConnection;
        private readonly ILogger _logger;

        private List<Organisation> _organisationList = new List<Organisation>();
        private List<Qualification> _qualificationsList = new List<Qualification>();

        public RegisterRepository(IDapperDbConnection dapperDbConnection, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RegisterRepository>();
            _dapperDbConnection = dapperDbConnection;
        }

        public async Task GetOrganisations()
        {
            using (var db = _dapperDbConnection.CreateConnection())

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


                    _logger.Log(LogLevel.Information, "Got Organisations");

                    _organisationList = organisations.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

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

        public Dictionary<string, object> GetData()
        {
            _logger.Log(LogLevel.Information, "Getting Data from DB");

            var thread1 = Task.Run(() => GetQualifications());
            var thread2 = Task.Run(() => GetOrganisations());

            Task.WaitAll(thread1, thread2);

            var dict = new Dictionary<string, object>
            {
                { "Qualifications", _qualificationsList },
                { "Organisations", _organisationList }
            };

            return dict;
        }
    }
}
