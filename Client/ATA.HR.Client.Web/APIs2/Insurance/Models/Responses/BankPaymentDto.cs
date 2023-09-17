namespace ATA.HR.Client.Web.APIs.Insurance.Models.Responses;

public class BankPaymentDto
{
    public int Id { get; set; }

    public int PayedByUserId { get; set; }

    public string? PayedByUserFullName { get; set; } 

    public int PaymentNo { get; set; }

    public DateTime? PayedAt { get; set; }

    public string? PayedAtJalali { get; set; } 

    public string? BankTrackingCode { get; set; }
}