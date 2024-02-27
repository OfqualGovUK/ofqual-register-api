using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Models.Private
{
    public class OrganisationPrivate
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? RecognitionNumber { get; set; }
        public string? LegalName { get; set; }
        public string? Acronym { get; set; }
        public string? OfqualOrganisationStatus { get; set; }
        public string? CceaOrganisationStatus { get; set; }
        public DateTime? OfqualRecognisedOn { get; set; }
        public DateTime? OfqualRecognisedTo { get; set; }
        public DateTime? OfqualSurrenderedOn { get; set; }
        public DateTime? OfqualWithdrawnOn { get; set; }
        public DateTime? CceaRecognisedOn { get; set; }
        public DateTime? CceaRecognisedTo { get; set; }
        public DateTime? CceaSurrenderedOn { get; set; }
        public DateTime? CceaWithdrawnOn { get; set; }
        public string? ContactEmail { get; set; }
        public string? Website { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FeesUrl { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressCity { get; set; }
        public string? AddressCounty { get; set; }
        public string? AddressCountry { get; set; }
        public string? AddressPostCode { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public OrganisationPrivate(Organisation organisation)
        {
            Id = organisation.Id;
            Name = organisation.Name;
            RecognitionNumber = organisation.RecognitionNumber;
            LegalName = organisation.LegalName;
            Acronym = organisation.Acronym;
            OfqualOrganisationStatus = organisation.OfqualOrganisationStatus;
            CceaOrganisationStatus = organisation.CceaOrganisationStatus;
            OfqualRecognisedOn = organisation.OfqualRecognisedOn;
            OfqualRecognisedTo = organisation.OfqualRecognisedTo;
            OfqualSurrenderedOn = organisation.OfqualSurrenderedOn;
            OfqualWithdrawnOn = organisation.OfqualWithdrawnOn;
            CceaRecognisedOn = organisation.CceaRecognisedOn;
            CceaRecognisedTo = organisation.CceaRecognisedTo;
            CceaSurrenderedOn = organisation.CceaSurrenderedOn;
            CceaWithdrawnOn = organisation.CceaWithdrawnOn;
            ContactEmail = organisation.ContactEmail;
            Website = organisation.Website;
            PhoneNumber = organisation.PhoneNumber;
            FeesUrl = organisation.FeesUrl;
            AddressLine1 = organisation.AddressLine1;
            AddressLine2 = organisation.AddressLine2;
            AddressCity = organisation.AddressCity;
            AddressCounty = organisation.AddressCounty;
            AddressCountry = organisation.AddressCountry;
            AddressPostCode = organisation.AddressPostCode;
            LastUpdatedDate = organisation.LastUpdatedDate;
        }

    }
}
