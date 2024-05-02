using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ofqual.Common.RegisterAPI.Models
{
    public class RecognitionScope
    {
        public required List<ScopeType> Inclusions { get; set; }
        public required List<ScopeType> Exclusions { get; set; }
    }

    public class ScopeType
    {
        public required string Type{ get; set; }
        public required List<ScopeLevel> Levels { get; set; }
    }

    public class ScopeLevel
    {
        public required string Level { get; set; }
        public required List<string> Recognitions { get; set; }
    }
}
