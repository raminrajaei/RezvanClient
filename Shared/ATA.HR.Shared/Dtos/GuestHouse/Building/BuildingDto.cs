using ATA.HR.Shared.Enums.GuestHouse;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BuildingDto
{
    [Required(ErrorMessage = "نام ساختمان را وارد کنید.")]
    public string? Title { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public int CityId { get; set; }

    public string? BuildingTypeSelectedValue { get; set; }

    public int BuildingType => BuildingTypeSelectedValue.IsNotNullOrEmpty() 
        ? (int)Enum.Parse(typeof(BuildingType), BuildingTypeSelectedValue)
        : 0;
}
