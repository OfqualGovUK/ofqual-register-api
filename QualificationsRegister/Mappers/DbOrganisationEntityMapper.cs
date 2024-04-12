using Ofqual.Common.RegisterAPI.Models;
using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Mappers
{
    public static class DbOrganisationEntityMapper
    {
        public static Organisation ToDomain(this DbOrganisation mDDBOrganisationEntity)
        {
            string apiUrl = Environment.GetEnvironmentVariable("APIMgmtURL")!;

            return new Organisation
            {
                Name = mDDBOrganisationEntity.Name,
                RecognitionNumber = mDDBOrganisationEntity.RecognitionNumber,
                LegalName = mDDBOrganisationEntity.LegalName,
                Acronym = mDDBOrganisationEntity.Acronym,
                OfqualOrganisationStatus = mDDBOrganisationEntity.OfqualOrganisationStatus,
                CceaOrganisationStatus = mDDBOrganisationEntity.CceaOrganisationStatus,
                OfqualRecognisedOn = mDDBOrganisationEntity.OfqualRecognisedOn?.ToUniversalTime(),
                OfqualRecognisedTo = mDDBOrganisationEntity.OfqualRecognisedTo?.ToUniversalTime(),
                OfqualSurrenderedOn = mDDBOrganisationEntity.OfqualSurrenderedOn?.ToUniversalTime(),
                OfqualWithdrawnOn = mDDBOrganisationEntity.OfqualWithdrawnOn?.ToUniversalTime(),
                CceaRecognisedOn = mDDBOrganisationEntity.CceaRecognisedOn?.ToUniversalTime(),
                CceaRecognisedTo = mDDBOrganisationEntity.CceaRecognisedTo?.ToUniversalTime(),
                CceaSurrenderedOn = mDDBOrganisationEntity.CceaSurrenderedOn?.ToUniversalTime(),
                CceaWithdrawnOn = mDDBOrganisationEntity.CceaWithdrawnOn?.ToUniversalTime(),
                ContactEmail = mDDBOrganisationEntity.ContactEmail,
                Website = mDDBOrganisationEntity.Website,
                PhoneNumber = mDDBOrganisationEntity.PhoneNumber,
                FeesUrl = mDDBOrganisationEntity.FeesUrl,
                AddressLine1 = mDDBOrganisationEntity.AddressLine1,
                AddressLine2 = mDDBOrganisationEntity.AddressLine2,
                AddressCity = mDDBOrganisationEntity.AddressCity,
                AddressCounty = mDDBOrganisationEntity.AddressCounty,
                AddressCountry = mDDBOrganisationEntity.AddressCountry,
                AddressPostCode = mDDBOrganisationEntity.AddressPostCode,
                LastUpdatedDate = mDDBOrganisationEntity.LastUpdatedDate.ToUniversalTime(),
                CanonicalUrl = $"{apiUrl}/api/organisations/{mDDBOrganisationEntity.RecognitionNumber}"
            };
        }

    }
}
