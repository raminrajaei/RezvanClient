using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Insurance.Models.Requests;

public class ConfirmCostCommand
{
    public int CostId { get; set; }

    [Required(ErrorMessage = "مبلغ تایید شده را وارد نمایید")]
    public int? ConfirmedAmount { get; set; }

    [Range(0, 100, ErrorMessage = "درصد فرانشیز بین 0 تا 100 می‌باشد")]
    public int FranchisePercent { get; set; }
}