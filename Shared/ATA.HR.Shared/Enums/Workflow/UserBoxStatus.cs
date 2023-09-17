using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Workflow;

public enum UserBoxStatus
{
    [Display(Name = "همه‌")]
    NotImportant = 0,

    [Display(Name = "دارد")]
    HasBox = 1,

    [Display(Name = "ندارد")]
    NoBox = 2
}