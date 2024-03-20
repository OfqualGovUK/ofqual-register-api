namespace Ofqual.Common.RegisterAPI.Models
{
    public class ListResponse<T>
    {
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int Limit { get; set; }

        public List<T>? Results { get; set; } = [];
    }
}
