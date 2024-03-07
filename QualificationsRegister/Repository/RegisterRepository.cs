using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Ofqual.Common.RegisterAPI.Models.DB;


namespace Ofqual.Common.RegisterAPI.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public RegisterRepository(string connectionString, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RegisterRepository>();
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Organisation>> GetOrganisations()
        {
            _logger.Log(LogLevel.Information, "Getting organisations data from DB");
            using (var db = new SqlConnection(_connectionString))

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

                    return organisations;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<IEnumerable<Qualification>> GetQualifications()
        {
            _logger.Log(LogLevel.Information, "Getting qualifications data from DB");
            using (var db = new SqlConnection(_connectionString))
            {
                try
                {
                    var qualifications =
                     await db.QueryAsync<Qualification>
                        (@"SELECT
                            [Id]
                            ,[QualificationNumber]
                            ,[QualifiationNumberNoObliques]
                            ,[Title]
                            ,[Status]
                            ,[OrganisationName]
                            ,[OrganisationAcronym]
                            ,[OrganisationRecognitionNumber]
                            ,[Type]
                            ,[SSA_Code] AS SSACode
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
                            ,[EmbargoDate]
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
                            ,[UILastUpdatedDate]
                            ,[InsertedDate]
                            ,[Version]
                            ,[AppearsOnPublicRegister]
                            ,[OrganisationId]
                            ,[LevelId]
                            ,[TypeId]
                            ,[SSAId]
                            ,[GradingTypeId]
                            ,[GradingScaleid]
                            ,[PreSixteen]
                            ,[SixteenToEighteen]
                            ,[EighteenPlus]
                            ,[NineteenPlus]
                          FROM [MD_Register].[Register_V_Qualification]");

                    _logger.Log(LogLevel.Information, "Got Qualifications");

                    return qualifications;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<object> GetDataAsync(string key)
        {
            _logger.Log(LogLevel.Information, "Getting {} data from DB", key);

            return key.ToLower() switch
            {
                "organisations" => await GetOrganisations(),
                "qualifications" => await GetQualifications(),
                _ => throw new NotImplementedException()
            };
        }
    }
}
