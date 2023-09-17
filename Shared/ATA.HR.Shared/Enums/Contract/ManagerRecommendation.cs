using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Contract;

public enum ManagerRecommendation
{
    [Display(Name = "عدم تمدید")]
    NoRenewal = 0,

    [Display(Name = "تمدید 1 ماهه")]
    OneMonth = 1,

    [Display(Name = "تمدید 3 ماهه")]
    ThreeMonth = 3,

    [Display(Name = "تمدید 6 ماهه")]
    SixMonth = 6
}