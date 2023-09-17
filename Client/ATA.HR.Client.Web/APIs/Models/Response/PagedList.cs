namespace ATA.HR.Client.Web.APIs.Models.Response;

public class PagedList<T> : PaginationOptions
    where T : class
{
    public PagedList()
    {
        Data = new List<T>();
    }

    public List<T> Data { get; set; }
}