using System.ComponentModel.DataAnnotations.Schema;

namespace Ofqual.Common.RegisterAPI.Models.DB
{
    public class Organisation
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string RecognitionNumber { get; set; }
        public required string LegalName { get; set; }
        public required string Acronym { get; set; }
        [Column("Ofqual_OrganisationStatus")]
        public string? OfqualOrganisationStatus { get; set; }
        [Column("CCEA_OrganisationStatus")]
        public string? CceaOrganisationStatus { get; set; }
        [Column("Ofqual_RecognisedOn")]
        public DateTime? OfqualRecognisedOn { get; set; }
        public DateTime? OfqualRecognisedTo { get; set; }
        [Column("Ofqual_SurrenderedOn")]
        public DateTime? OfqualSurrenderedOn { get; set; }
        [Column("Ofqual_WithdrawnOn")]
        public DateTime? OfqualWithdrawnOn { get; set; }
        [Column("CCEA_RecognisedOn")]
        public DateTime? CceaRecognisedOn { get; set; }
        [Column("CCEA_RecognisedTo")]
        public DateTime? CceaRecognisedTo { get; set; }
        [Column("CCEA_SurrenderedOn")]
        public DateTime? CceaSurrenderedOn { get; set; }
        [Column("CCEA_WithdrawnOn")]
        public DateTime? CceaWithdrawnOn { get; set; }
        public required string ContactEmail { get; set; }
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
    }
}
