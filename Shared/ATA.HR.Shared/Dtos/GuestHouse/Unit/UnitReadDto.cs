using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class UnitReadDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public int RoomCounts { get; set; }

    public string? Facilities { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public string IsActiveString => IsActive ? "فعال" : "غیرفعال";
}
