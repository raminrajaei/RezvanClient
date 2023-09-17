namespace ATA.HR.Client.Web.APIs.HRInsuranceModels;

public class PolicyObligationDto
{
    public int Id { get; set; }

    public int PolicyId { get; set; }

    public string? PolicyTitle { get; set; }

    public int ObligationCode { get; set; }

    public string? ObligationTitle { get; set; }

    public string? ObligationDescription { get; set; }

    public int MaxCoverage { get; set; }
}