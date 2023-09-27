using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum MaritalStatusEnum
{
    [Display(Name = "مجرد")]
    Single = 1,

    [Display(Name = "متاهل")]
    Married = 2
}