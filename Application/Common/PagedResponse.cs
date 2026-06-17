namespace EcommerceApp.Application.Common
{
    public class PagedResponse<T> : ApiResponses<IEnumerable<T>>
    {
        public int PageNumer { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
