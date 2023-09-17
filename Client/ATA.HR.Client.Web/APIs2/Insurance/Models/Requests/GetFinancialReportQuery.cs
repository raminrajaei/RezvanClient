namespace ATA.HR.Client.Web.APIs.Insurance.Models.Requests;

public class GetFinancialReportQuery
{
    public int? BankPaymentId { get; set; }

    public string? FromConfirmJalaliDate { get; set; }

    public string? ToConfirmJalaliDate { get; set; }
}