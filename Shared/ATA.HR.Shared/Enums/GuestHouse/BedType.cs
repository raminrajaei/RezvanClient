using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.GuestHouse;

public enum BedType
{
    [Display(Name = "یک نفره")]
    Single = 1,

    [Display(Name = "دونفره")]
    Double = 2,

    [Display(Name = "کینگ")]
    King = 3
}
