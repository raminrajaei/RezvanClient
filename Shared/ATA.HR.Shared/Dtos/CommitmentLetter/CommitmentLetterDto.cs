using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.CommitmentLetter;

[ComplexType]
public class CommitmentLetterDto : IValidatableObject
{
    [Required(ErrorMessage = "ابتدا یکی از کاربران را از لیست انتخاب نمایید")]
    public string? UserIdEmployeeSelectedValue { get; set; }

    [Required(ErrorMessage = "لطفا عنوانی برای تعهدنامه مشخص کنید")]
    public string? Title { get; set; } = "سند تعهدنامه‌ی غیرمالی";

    public string? CommitmentLetterNo { get; set; } //شماره تعهدنامه محضری

    public string? CommitmentLetterRegisteredAtJalali { get; set; } //تاریخ تعهد محضری

    [Required(ErrorMessage = "تاریخ شروع تعهدنامه را مشخص نمایید")]
    public string? CommitmentStartsAtJalali { get; set; }

    [Required(ErrorMessage = "تاریخ پایان تعهدنامه را مشخص نمایید")]
    public string? CommitmentEndsAtJalali { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CommitmentLetterRegisteredAtJalali.IsNotNullOrEmpty() && CommitmentLetterRegisteredAtJalali!.IsStringInValidDateFormat() is false)
            yield return new ValidationResult("فرمت تاریخ تعهد محضری معتبر نیست. تاریخ به صورت 1400/01/01 معتبر می‌باشد",
                new List<string> { nameof(CommitmentLetterRegisteredAtJalali) });

        if (CommitmentStartsAtJalali!.IsStringInValidDateFormat() is false)
            yield return new ValidationResult("فرمت تاریخ شروع تعهدنامه معتبر نیست. تاریخ به صورت 1400/01/01 معتبر می‌باشد",
                new List<string> { nameof(CommitmentStartsAtJalali) });

        if (CommitmentEndsAtJalali!.IsStringInValidDateFormat() is false)
            yield return new ValidationResult("فرمت تاریخ پایان تعهدنامه معتبر نیست. تاریخ به صورت 1400/01/01 معتبر می‌باشد",
                new List<string> { nameof(CommitmentEndsAtJalali) });

        bool compareDates = CommitmentStartsAtJalali!.IsStringInValidDateFormat() && CommitmentEndsAtJalali!.IsStringInValidDateFormat();

        if (compareDates && CommitmentStartsAtJalali!.IsThisDateAfterThan(CommitmentEndsAtJalali!))
            yield return new ValidationResult("تاریخ شروع تعهدنامه نمی‌تواند بعد از تاریخ پایان آن باشد",
                new List<string> { nameof(CommitmentStartsAtJalali) });

    }
}