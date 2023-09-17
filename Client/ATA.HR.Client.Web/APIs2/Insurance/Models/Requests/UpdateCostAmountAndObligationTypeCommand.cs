using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Insurance.Models.Requests;

public class UpdateCostAmountAndObligationTypeCommand
{
    public int CostId { get; set; }

    [Required(ErrorMessage = "مبلغ پرداخت شده را وارد نمایید")]
    public int? PaidAmount { get; set; }

    [Required(ErrorMessage = "یک عنوان انتخاب نمایید")]
    public int? PolicyObligationId { get; set; }
}