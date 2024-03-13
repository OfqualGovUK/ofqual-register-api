using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.Database
{
    public interface IRegisterDb
    {
        List<Organisation>? GetOrganisationsList(string name);
        Organisation? GetOrganisationByNumber(string number, string numberRN);
        Task<List<Qualification>> GetQualifications(string search = "");
        Task<List<QualificationPublic>> GetQualificationsPublic(string search = "");

    }
}