namespace ATA.HR.Client.Web.APIs.Insurance.Models.Responses;

public class InsuranceFlowFormsDto
{
    public bool IsCurrentUserActor { get; set; }

    public bool IsFlowFinished { get; set; }

    public bool HasFlowJustStarted { get; set; }

    public CostDto? Cost { get; set; }
}