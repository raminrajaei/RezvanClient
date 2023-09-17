using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dto;

[ComplexType]
public class BedDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool IsActive { get; set; } = true;
}
