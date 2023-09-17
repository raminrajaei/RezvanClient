using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Insurance.Models.Requests;

public class GetCostsQuery : IValidatableObject
{
    public string? SearchTerm { get; set; }

    public int? PolicyObligationId { get; set; }

    public string? WorkLocation { get; set; }

    public string? FromCostJalaliDate { get; set; }

    public string? ToCostJalaliDate { get; set; }

    public string? FromConfirmJalaliDate { get; set; }

    public string? ToConfirmJalaliDate { get; set; }

    public string? FlowCurrentStateTag { get; set; }

    public string? CostCurrentStatusSelectedValue { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FromCostJalaliDate.IsNotNullOrEmpty())
        {
            if (FromCostJalaliDate!.IsStringInValidDateFormat() is false)
            {
                yield return new ValidationResult("فرمت تاریخ 'از' معتبر نیست",
                    new[] { nameof(FromCostJalaliDate) });
            }
        }

        if (ToCostJalaliDate.IsNotNullOrEmpty())
        {
            if (ToCostJalaliDate!.IsStringInValidDateFormat() is false)
            {
                yield return new ValidationResult("فرمت تاریخ 'تا' معتبر نیست",
                    new[] { nameof(ToCostJalaliDate) });
            }
        }

        if (FromConfirmJalaliDate.IsNotNullOrEmpty())
        {
            if (FromConfirmJalaliDate!.IsStringInValidDateFormat() is false)
            {
                yield return new ValidationResult("فرمت تاریخ 'از' معتبر نیست",
                    new[] { nameof(FromConfirmJalaliDate) });
            }
        }

        if (ToConfirmJalaliDate.IsNotNullOrEmpty())
        {
            if (ToConfirmJalaliDate!.IsStringInValidDateFormat() is false)
            {
                yield return new ValidationResult("فرمت تاریخ 'تا' معتبر نیست",
                    new[] { nameof(ToConfirmJalaliDate) });
            }
        }
    }
}