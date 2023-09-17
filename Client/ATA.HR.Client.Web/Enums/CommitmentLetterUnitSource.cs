using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.Enums;

public enum CommitmentLetterUnitSource
{
    [Display(Name = "عملیات")]
    Operation,

    [Display(Name = "تعمیرات")]
    Repairs
}