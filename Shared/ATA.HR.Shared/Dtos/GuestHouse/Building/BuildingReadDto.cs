using ATA.HR.Shared.Enums.GuestHouse;
using ATABit.Helper.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class BuildingReadDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public int BuildingType { get; set; }
    public string? BuildingTypeDisplay => ((BuildingType)BuildingType).ToDisplayName();
}
