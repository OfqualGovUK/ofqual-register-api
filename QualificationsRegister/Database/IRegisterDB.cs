using Ofqual.Common.RegisterAPI.Models.DB;
using Ofqual.Common.RegisterAPI.Models;

namespace Ofqual.Common.RegisterAPI.Database
{
    public interface IRegisterDb
    {
        DbListResponse<DbOrganisation>? GetOrganisationsList(int page, int? limit, string name);
        DbOrganisation? GetOrganisationByNumber(string number, string numberRN);

        DbListResponse<DbQualification> GetQualificationsList(int page, int limit, QualificationFilter? query, string title);
        DbListResponse<DbQualificationPublic> GetQualificationsPublicList(int page, int limit, QualificationFilter? query, string title);

        DbQualification? GetQualificationByNumber(string numberObliques, string numberNoObliques);
        DbQualificationPublic? GetQualificationPublicByNumber(string numberObliques, string numberNoObliques);

        List<DbRecognitionScope> GetRecognitionScope(string organisationNumber);

    }
}
