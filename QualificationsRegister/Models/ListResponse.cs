namespace Ofqual.Common.RegisterAPI.Models
{
    //unifies the list response for the JSON respose for anything with a list 
    public class ListResponse<T>
    {
        public int Count { get; set; }
        public int? CurrentPage { get; set; }
        public int? Limit { get; set; }

        public List<T>? Results { get; set; } = [];
    }
}
