using ATABit.Helper.Extensions;
using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Insurance.Models.Requests;

public class SetCostCommand
{
    public int? CostId { get; set; }

    [Required(ErrorMessage = "هیچ بیمه شده‌ای انتخاب نشده است")]
    public int? InsuredId { get; set; }

    [Required(ErrorMessage = "هیچ تعهد بیمه‌ای انتخاب نشده است")]
    public int? PolicyObligationId { get; set; }

    [Required(ErrorMessage = "عنوان هزینه را وارد نمایید")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "تاریخ هزینه را وارد نمایید")]
    public string? CostDateJalali { get; set; }

    [Required(ErrorMessage = "مبلغ پرداختی را وارد نمایید")]
    public int? PaidAmount { get; set; }

    public string? Description { get; set; }

    public List<CostAttachmentCommand> CostAttachments { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CostDateJalali.IsValidPersianDateTime() is false)
            yield return new ValidationResult("فرمت تاریخ ثبت هزینه اشتباه است", new List<string> { nameof(CostDateJalali) });
        else
        {
            if (CostDateJalali!.IsThisDateAfterThan("1401/01/01") is false)
                yield return new ValidationResult("تاریخ هزینه بعد از ۱۴۰۲/۰۱/۰۱ معتبر می‌باشد", new List<string> { nameof(CostDateJalali) });
        }

        if (CostAttachments.Count == 0)
            yield return new ValidationResult("هیچ پیوستی وارد نشده است");
    }
}

public class CostAttachmentCommand
{
    [Required(ErrorMessage = "عنوان پیوست را مشخص نمایید")]
    public string? Title { get; set; }

    public string? FileType { get; set; }

    public string? FileExtension { get; set; }

    [Required(ErrorMessage = "هیچ فایل پیوستی وجود ندارد")]
    public string? Url { get; set; }
}