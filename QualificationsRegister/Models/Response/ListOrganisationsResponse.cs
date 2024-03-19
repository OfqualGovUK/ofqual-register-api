namespace Ofqual.Common.RegisterAPI.Models.Response
{
    public class ListOrganisationsResponse
    {
        public List<Organisation>? Organisations { get; set; }
        public string? NextPage { get; set; }
        public int Count { get; set; }
        public int CurrentPage {  get; set; }
        public int Limit { get; set; }
    }
}
