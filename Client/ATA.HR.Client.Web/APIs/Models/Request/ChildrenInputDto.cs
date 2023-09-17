namespace ATA.HR.Client.Web.APIs.Models.Request;

public class ChildrenInputDto
{
    public string? SearchTerm { get; set; }
    public int? Year { get; set; } // as year of birth
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

}