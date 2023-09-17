using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.GuestHouse;

public enum RoomBookingStatus
{
    [Display(Name = "همه")]
    All = 0,

    [Display(Name = "خالی")]
    Empty = 1,

    [Display(Name = "ظرفیت تکمیل")]
    FullCapacity = 2,

    [Display(Name = "دارای ظرفیت")]
    HasCapacity = 3,
}
