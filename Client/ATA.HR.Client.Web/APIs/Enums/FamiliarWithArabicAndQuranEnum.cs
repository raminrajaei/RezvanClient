using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum FamiliarWithArabicAndQuranEnum
{
    [Display(Name = "ضعيف")]
    Weak = 1,

    [Display(Name = "متوسط")]
    Medium = 2,

    [Display(Name = "خوب")]
    Good = 3,

    [Display(Name = "خيلي خوب")]
    VeryGood = 4,

    [Display(Name = "عالي")]
    Great = 5
}