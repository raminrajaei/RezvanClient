using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class RoomDto
{
    [Required(ErrorMessage ="نام اتاق را وارد کنید."), StringLength(200)]
    public string? Title { get; set; }

    public int BedCounts { get; set; }

    public int Floor { get; set; }

    public string? Facilities { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int BuildingId { get; set; }

    public int UnitId { get; set; }
}
