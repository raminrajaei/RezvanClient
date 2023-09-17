namespace ATA.HR.Client.Web.APIs.Insurance.Models.Requests;

public class GetRemainedBalanceQuery
{
    public int InsuredId { get; set; }

    public int PolicyObligationId { get; set; }

    public int? CostId { get; set; }
}