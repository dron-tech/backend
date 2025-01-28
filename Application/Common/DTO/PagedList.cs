namespace Application.Common.DTO;

public class PagedList<T> where T : class
{
    public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
