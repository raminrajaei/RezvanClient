using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class RoomFilterArgs
{
    public int BuildingId { get; set; }

    public int UnitId { get; set; }

    public string? SearchTerm { get; set; }
}
