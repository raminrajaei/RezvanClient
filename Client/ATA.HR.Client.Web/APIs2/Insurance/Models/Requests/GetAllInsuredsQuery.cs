namespace ATA.HR.Client.Web.APIs.Insurance.Models;

public class GetAllInsuredsQuery
{
    public string? SearchTerm { get; set; }

    public bool OnlyMainInsureds { get; set; }

    public bool OnlyDependents { get; set; }

    public bool OnlyCancelRequests { get; set; }

    public int? InsuranceStatus { get; set; }

    public int? LifeInsuranceStatus { get; set; }

    public string? FromEndedAtJalali { get; set; }

    public string? ToEndedAtJalali { get; set; }
}
