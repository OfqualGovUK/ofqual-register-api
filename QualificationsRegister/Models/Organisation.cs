using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ofqual.Common.RegisterAPI.Models
{
    public class Organisation
    {
        public required string Name { get; set; }

        public required string RecognitionNumber { get; set; }

        public required string LegalName { get; set; }

        public required string Acronym { get; set; }

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

        public string? CanonicalUrl { get; set;}
    }
}
