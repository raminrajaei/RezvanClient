using ATA.HR.Shared.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class RoomReadDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int BedCounts { get; set; }

    public int OccupiedBedCounts { get; set; }

    public bool AreAllBedsBooked { get; set; }

    public int Floor { get; set; }

    public string? Facilities { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public string IsActiveString => IsActive ? "فعال" : "غیرفعال";

    public string? BuildingName { get; set; }

    public int BuildingId { get; set; }

    public string? UnitName { get; set; }

    public int UnitId { get; set; }

    public List<BedDto> Beds { get; set; } = new();
}
