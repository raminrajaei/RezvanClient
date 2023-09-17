using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.GuestHouse;

public enum BuildingType
{
    [Display(Name = "اتاق")]
    Room = 1,

    [Display(Name = "واحد")]
    Unit = 2
}
