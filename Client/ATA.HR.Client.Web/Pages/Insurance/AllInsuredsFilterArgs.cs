namespace ATA.HR.Client.Web.Pages.Insurance;

public class AllInsuredsFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? InsuranceStatusSelectedValue { get; set; }

    public string? LifeInsuranceStatusSelectedValue { get; set; }

    public string? OnlyMainInsuredsOrDependentsSelectedValue { get; set; }

    public string? OnlyCancelRequestsSelectedValue { get; set; }

    public string? FromEndedAtJalali { get; set; }

    public string? ToEndedAtJalali { get; set; }
}
