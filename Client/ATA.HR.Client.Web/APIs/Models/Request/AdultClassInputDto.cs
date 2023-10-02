namespace ATA.HR.Client.Web.APIs.Models.Request;

public class AdultClassInputDto
{
    public string SearchTerm { get; set; }
    public string? Year { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}