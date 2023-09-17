using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class UnitDto
{
    [Required(ErrorMessage ="نام واحد را وارد کنید."), StringLength(200)]
    public string? Title { get; set; }

    [Range(1, 100, ErrorMessage = "تعداد اتاق های این واحد را مشخص کنید.")]
    public int RoomCounts { get; set; }

    public string? Facilities { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public int BuildingId { get; set; }
}
