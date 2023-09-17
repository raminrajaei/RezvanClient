namespace ATA.HR.Client.Web.APIs.Insurance.Models.Requests;

public class SaveBankPaymentCommand
{
    public List<int> PersonnelCodes { get; set; } = new();

    public string PayedAtJalali { get; set; }

    public string BankTrackingNo { get; set; }
}