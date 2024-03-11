using Ofqual.Common.RegisterAPI.Models.DB;

namespace Ofqual.Common.RegisterAPI.Database
{
    public interface IRegisterDb
    {
        public List<MDDBOrganisation> GetOrganisationsList(string number, string name);
        public MDDBOrganisation GetOrganisationByNumber(string number);
        public Task<List<Qualification>> GetQualifications(string search = "");
        public Task<List<QualificationPublic>> GetQualificationsPublic(string search = "");

    }
}
