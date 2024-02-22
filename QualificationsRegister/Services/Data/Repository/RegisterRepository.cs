using Dapper;
using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Services.Data
{
    public class RegisterRepository : IRegisterRepository
    {
        public IDapperDbConnection _dapperDbConnection;
        public RegisterRepository(IDapperDbConnection dapperDbConnection)
        {
            _dapperDbConnection = dapperDbConnection;
        }

        public async Task<IEnumerable<Organisation>> GetAllOrganisationsAsync()
        {
            using (IDbConnection db = _dapperDbConnection.CreateConnection())
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

                    return organisations;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }
}
