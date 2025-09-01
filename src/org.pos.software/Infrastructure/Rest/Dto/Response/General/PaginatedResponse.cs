namespace org.pos.software.Infrastructure.Rest.Dto.Response.General
{
    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
