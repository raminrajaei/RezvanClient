namespace ATA.HR.Client.Web.APIs.Models.Response;

public class PaginationOptions
{
    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public int TotalPages
    {
        get
        {
            var ceiling = Math.Ceiling(TotalCount / (double)PageSize);
            return (int)ceiling;
        }
    }

    public int CurrentPage { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}