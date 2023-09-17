using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Contract;

public enum HokmTypeStatusToSendContract
{
    [Display(Name = "همه‌")]
    NotImportant = 0,

    [Display(Name = "دارد")]
    Allowed = 1,

    [Display(Name = "ندارد")]
    NotAllowed = 2
}