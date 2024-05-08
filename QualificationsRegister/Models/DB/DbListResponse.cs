namespace Ofqual.Common.RegisterAPI.Models
{
    public class DbListResponse<T>
    {
        public int Count { get; set; }     

        public List<T>? Results { get; set; } = [];
    }
}
