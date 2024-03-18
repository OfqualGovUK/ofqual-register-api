using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.Database
{
    public interface IRegisterDb
    {
        List<Organisation>? GetOrganisationsList(string name);
        Organisation? GetOrganisationByNumber(string number, string numberRN);

        ListResponse<Qualification> GetQualificationsByName(int page, int limit, string title);
        ListResponse<QualificationPublic> GetQualificationsPublicByName(int page, int limit, string title);

        Qualification? GetQualificationByNumber(string numberObliques, string numberNoOblique);
        QualificationPublic? GetQualificationPublicByNumber(string numberObliques, string numberNoObliques);

    }
}
