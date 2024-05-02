using Ofqual.Common.RegisterFrontend.Models.APIModels;
using Refit;

namespace Ofqual.Common.RegisterFrontend.RegisterAPI
{
    public interface IRefDataAPIClient
    {
        [Get("/qualificationtypes")]
        Task<List<QualificationType>> GetQualificationTypesAsync();

        [Get("/levels")]
        Task<List<Level>> GetLevelsAsync();

    }
}
