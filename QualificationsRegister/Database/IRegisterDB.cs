using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.Database
{
    public interface IRegisterDb
    {
        ListResponse<Organisation> GetOrganisationsList(int limit, int offSet, string name);
        Organisation? GetOrganisationByNumber(string number, string numberRN);

        ListResponse<Qualification> GetQualificationsByName(int page, int limit, QualificationFilter? query, string title);
        ListResponse<QualificationPublic> GetQualificationsPublicByName(int page, int limit, QualificationFilter? query, string title);

        Qualification? GetQualificationByNumber(string numberObliques, string numberNoObliques);
        QualificationPublic? GetQualificationPublicByNumber(string numberObliques, string numberNoObliques);

    }
}
