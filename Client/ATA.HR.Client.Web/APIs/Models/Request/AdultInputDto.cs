namespace ATA.HR.Client.Web.APIs.Models.Request;

public class AdultInputDto
{
    public string SearchTerm { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}