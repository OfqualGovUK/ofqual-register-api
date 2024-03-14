using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.Database
{
    public interface IRegisterDb
    {
        List<Organisation>? GetOrganisationsList(string name);
        Organisation? GetOrganisationByNumber(string number, string numberRN);

        List<Qualification> GetQualificationsByName(string title);
        List<QualificationPublic> GetQualificationsPublicByName(string title);

        Qualification? GetQualificationByNumber(string numberObliques, string numberNoOblique);
        QualificationPublic? GetQualificationPublicByNumber(string numberObliques, string numberNoObliques);

    }
}
