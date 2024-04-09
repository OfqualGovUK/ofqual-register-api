using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.Database
{
    public interface IRegisterDb
    {
        DbListResponse<DbOrganisation>? GetOrganisationsList(int limit, int offSet, string name);
        DbOrganisation? GetOrganisationByNumber(string number, string numberRN);

        DbListResponse<DbQualification> GetQualificationsByName(int page, int limit, QualificationFilter? query, string title);
        DbListResponse<DbQualificationPublic> GetQualificationsPublicByName(int page, int limit, QualificationFilter? query, string title);

        DbQualification? GetQualificationByNumber(string numberObliques, string numberNoObliques);
        DbQualificationPublic? GetQualificationPublicByNumber(string numberObliques, string numberNoObliques);

    }
}
