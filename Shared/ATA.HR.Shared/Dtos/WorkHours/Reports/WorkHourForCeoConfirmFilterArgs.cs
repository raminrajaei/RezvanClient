using DNTPersianUtils.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.WorkHours.Reports;

[ComplexType]
public class WorkHourForCeoConfirmFilterArgs
{
    [Required(ErrorMessage = "سال انتخاب نشده است")]
    public string? YearSelectedValue { get; set; } = DateTime.Now.GetPersianYear().ToString();

    [Required(ErrorMessage = "ماه انتخاب نشده است")]
    public string? MonthSelectedValue { get; set; } = DateTime.Now.GetPersianMonth().ToString();

    [Required(ErrorMessage = "مدیر مستقیم انتخاب نشده است")]
    public string? ConfirmerUserIdSelectedValue { get; set; }
}