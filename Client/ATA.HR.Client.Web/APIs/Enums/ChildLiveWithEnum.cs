using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.APIs.Enums;

public enum ChildLiveWithEnum
{
    [Display(Name = "والدین")]
    Parents,

    [Display(Name = "پدر و نامادری")]
    FatherAndStepmother,

    [Display(Name = "مادر و ناپدری")]
    MotherAndStepfather,

    [Display(Name = "فقط مادر")]
    Mother,

    [Display(Name = "فقط پدر")]
    Father,

    [Display(Name = "سایر بستگان")]
    Other
}